using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody body;
    private Animator anim;
    [SerializeField] private float speed;
    [SerializeField] private GameController Controller;


    private Dictionary<String, String[]> compatibility;
    public LayerMask mask;
    [Tooltip("End of the horizontal raycast")] public GameObject chestPoint;
    [SerializeField] private float raycastSeparation = 0.35f; //dist?ncia del raycast, jugar amb valors
   
    private GameObject carry;
    private bool collision;
    private Quaternion rotateAngle;
    private Vector3 relativePos;

    [Header("Particles Stuff")]
    public ParticleSystem ps;
    public AudioManager au;
    public int nParticles;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        initCompatibilities();
    }

    private void initCompatibilities()
    {
        compatibility = new Dictionary<string, string[]>();
        compatibility.Add("Olla", new String[]      { "Tomato", "Mushroom" });
        compatibility.Add("Plate", new String[]     { "Cheese","Steak","Mushroom","Lettuce","Tomato","Dough","Sausage","Bread" });
        compatibility.Add("Sarten", new String[]    { "Steak" });

    }

    private void CloseBoxes()
    {
        GameObject.Find("BurgerBunsBoxTop").GetComponent<Animator>().SetBool("Tick", false);
        GameObject.Find("CheeseBoxtop").GetComponent<Animator>().SetBool("Tick", false);
    }
    private void avoidDrift()
    {
        body.velocity = new Vector3(0, 0, 0);
        body.angularVelocity = new Vector3(0, 0, 0);
    }
    void Update()
    {
        avoidDrift();
        
        Vector3 relativePosRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (relativePosRaw != new Vector3(0, 0, 0))
        {
            relativePos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
        //Debug.Log(relativePos);
        relativePos.Normalize();
        Ray raig = new Ray(transform.position, relativePos ); //segon parametre es la direccio del raig
        RaycastHit colisio;

        if (Physics.Raycast(raig, out colisio, 2f))
        {
            collision = true;
            Debug.Log(colisio.collider.name);
            if (carry != null && Input.GetKeyDown("space")) leaveItem(colisio); //&& placable(colisio)
            else if (carry == null && Input.GetKeyDown("space")) pickItem(colisio);

            if (carry == null && colisio.collider.tag == "cuttingBoard" && colisio.collider.GetComponent<CuttingTable>().hasItem() && Input.GetKey(KeyCode.E)) colisio.collider.GetComponent<CuttingTable>().setCutting();
            //Pintar de diferente color depende del ingrediente.
            if (colisio.collider.tag.Equals("mesa")) Debug.DrawRay(transform.position, relativePos * 3, Color.red);
            if (colisio.collider.tag.Equals("BreadBox")) Debug.DrawRay(transform.position, relativePos * 3, Color.yellow);
            if (colisio.collider.tag.Equals("CheeseBox")) Debug.DrawRay(transform.position, relativePos * 3, Color.blue);
        }
        else
        {
            collision = false;
            Debug.DrawRay(transform.position, relativePos * 1000, Color.white);
        }
    }

    private void leaveItem(RaycastHit colisio)
    {
        Debug.Log("Tried to leave item in " + colisio.collider.name);
        switch (colisio.collider.tag)
        {
            case "mesa":  //encimera
                Encimera enc = colisio.collider.GetComponent<Encimera>();
                
                if (!enc.hasItem()) { // Deixem carrega.
                    carry.transform.parent = colisio.collider.transform;
                    colisio.collider.GetComponent<Encimera>().setItem(carry);
                    carry = null;
                }
                else if ( isContainer(carry.tag) && enc.hasIngredient() && isCompatible(carry, enc.getItem2check()) ) { // Llevo Contenedor Recogo Ingrediente
                    if(carry.tag == "Olla" && enc.isPlateReady() && !carry.GetComponent<Olla>().isFull())
                        carry.GetComponent<Olla>().setItem(enc.getItem());
                    else if (carry.tag == "Sarten" && enc.isCookable() && !carry.GetComponent<Sarten>().isFull())
                        carry.GetComponent<Sarten>().setItem(enc.getItem());
                    else if(enc.isPlateReady() && !carry.GetComponent<Plate>().isFull() && !carry.GetComponent<Plate>().isLocked())
                        carry.GetComponent<Plate>().setItem(enc.getItem());
                }
                else if ( isIngredient(carry.tag) && enc.hasContainer() && isCompatible(enc.getItem2check(), carry)  && enc.hasContainerSpace()) {
                    
                    if ( enc.getItemTag() == "Olla" && carry.GetComponent<Ingredient>().isPlateReady())
                    {
                        GameObject item = enc.getItem();
                        item.GetComponent<Olla>().setItem(carry);
                        enc.setItem(item);
                        carry = null;
                    }
                    else if (enc.getItemTag() == "Sarten" && carry.GetComponent<Ingredient>().isCookable())
                    {
                        GameObject item = enc.getItem();
                        item.GetComponent<Sarten>().setItem(carry);
                        enc.setItem(item);
                        carry = null;
                    }                        
                    else if (enc.getItemTag() == "Plate" && carry.GetComponent<Ingredient>().isPlateReady())
                    {
                        GameObject item = enc.getItem();
                        item.GetComponent<Plate>().setItem(carry);
                        enc.setItem(item);
                        carry = null;
                    }  
                }
                else if (isContainer(carry.tag) && isContainer(enc.getItem2check().tag))
                {
                    GameObject cont = enc.getItem2check();
                    if (carry.tag == "Plate" && carry.GetComponent<Plate>().isEmpty() && cont.tag == "Olla" && cont.GetComponent<Olla>().isFull() && cont.GetComponent<Olla>().isDoneCooking() && !cont.GetComponent<Olla>().isBurned())
                    {
                        carry.GetComponent<Plate>().setSopa(cont.GetComponent<Olla>().getItems());
                    }
                    else if (carry.tag == "Olla" && carry.GetComponent<Olla>().isFull() && carry.GetComponent<Olla>().isDoneCooking() && !carry.GetComponent<Olla>().isBurned()
                        && cont.tag == "Plate" && cont.GetComponent<Plate>().isEmpty())
                    {
                        cont.GetComponent<Plate>().setSopa(carry.GetComponent<Olla>().getItems());
                    }
                    else if (carry.tag == "Plate" && !carry.GetComponent<Plate>().isFull() && !cont.GetComponent<Plate>().isLocked() 
                        && cont.tag == "Sarten" && cont.GetComponent<Sarten>().isFull() && cont.GetComponent<Sarten>().isDoneCooking() && !cont.GetComponent<Sarten>().isBurned())
                    {
                        carry.GetComponent<Plate>().setItem(cont.GetComponent<Sarten>().getItem());
                    }
                    else if (carry.tag == "Sarten" && carry.GetComponent<Sarten>().isFull() && carry.GetComponent<Sarten>().isDoneCooking() && !carry.GetComponent<Sarten>().isBurned() &&
                             cont.tag == "Plate" && !cont.GetComponent<Plate>().isFull() && !cont.GetComponent<Plate>().isLocked())
                    {
                        enc.getItem2check().GetComponent<Plate>().setItem(carry.GetComponent<Sarten>().getItem());
                    }
                }
                break;
            case "fogon":
                Fogon fog = colisio.collider.GetComponent<Fogon>();
                if (!fog.hasItem() && (carry.tag == "Sarten" || carry.tag == "Olla")) { // Soltar Sarten/Olla
                    carry.transform.parent = colisio.collider.transform;
                    colisio.collider.GetComponent<Fogon>().setItem(carry);
                    carry = null;
                }
                else if (fog.hasItem() && isIngredient(carry.tag) && fog.hasContainerSpace() && isCompatible(fog.getItem2check(),carry))
                {
                    if (carry.GetComponent<Ingredient>().isCookable())
                    {
                        GameObject cont = fog.getItem();
                        cont.GetComponent<Sarten>().setItem(carry);
                        fog.setItem(cont);
                        carry = null;
                    }
                    else if (carry.GetComponent<Ingredient>().isPlateReady())
                    {
                        GameObject cont = fog.getItem();
                        cont.GetComponent<Olla>().setItem(carry);
                        fog.setItem(cont);
                        carry = null;
                    }  
                }
                else if ( fog.hasItem() && (carry.tag == "Sarten" || carry.tag == "Olla"))
                {
                    GameObject item = fog.getItem();
                    fog.setItem(carry);
                    carry.transform.parent = fog.transform;
                    carry = item;
                    carry.transform.parent = transform;
                }
                else if (fog.hasItem() && carry.tag == "Plate")
                {
                    GameObject item = fog.getItem2check();
                    if(item.tag == "Sarten" && item.GetComponent<Sarten>().isFull() && fog.isDoneCooking() && !item.GetComponent<Sarten>().isBurned() && !carry.GetComponent<Plate>().isLocked() && !carry.GetComponent<Plate>().isFull())
                        carry.GetComponent<Plate>().setItem(item.GetComponent<Sarten>().getItem());
                    else if (item.tag == "Olla" && item.GetComponent<Olla>().isFull() && fog.isDoneCooking() && !item.GetComponent<Olla>().isBurned() && carry.GetComponent<Plate>().isEmpty())
                        carry.GetComponent<Plate>().setSopa(item.GetComponent<Olla>().getItems());
                }
                break;
            case "Horno":
                Oven oven = colisio.collider.GetComponent<Oven>();
                if (carry.tag == "Plate" && carry.GetComponent<Plate>().has_Dough() && oven.isEmpty())
                {
                    carry.transform.parent = colisio.collider.transform;
                    oven.setItems(carry.GetComponent<Plate>().getItems());
                    //function de cocinar
                }
                else if (carry.tag == "Plate" && carry.GetComponent<Plate>().isEmpty() && !oven.isEmpty() && oven.isDoneCooking() && oven.onFire())
                {
                    if (oven.isBurned()) carry.GetComponent<Plate>().setBurnedFood();
                    carry.GetComponent<Plate>().setPizza(oven.getItems());                    
                }
                break;
            case "cuttingBoard":
                if (isIngredient(carry.tag) && carry.GetComponent<Ingredient>().isCuttable() && !colisio.collider.GetComponent<CuttingTable>().hasItem())
                {
                    carry.transform.parent = colisio.collider.transform;
                    colisio.collider.GetComponent<CuttingTable>().setItem(carry);
                    carry = null;
                }
                else if ( carry.tag == "Plate" && !carry.GetComponent<Plate>().isFull() && !carry.GetComponent<Plate>().isLocked() && colisio.collider.GetComponent<CuttingTable>().hasItem() && colisio.collider.GetComponent<CuttingTable>().isPlateReady())
                {
                    GameObject item = colisio.collider.GetComponent<CuttingTable>().getItem();
                    carry.GetComponent<Plate>().setItem(item);
                }
                else if (carry.tag == "Sarten" && !carry.GetComponent<Sarten>().isFull() && colisio.collider.GetComponent<CuttingTable>().hasItem() && colisio.collider.GetComponent<CuttingTable>().isCookable())
                {
                    GameObject item = colisio.collider.GetComponent<CuttingTable>().getItem();
                    carry.GetComponent<Sarten>().setItem(item);
                }
                else if (carry.tag == "Olla" && !carry.GetComponent<Olla>().isFull() && !carry.GetComponent<Olla>().isBurned() && colisio.collider.GetComponent<CuttingTable>().hasItem() && isCompatible(carry, colisio.collider.GetComponent<CuttingTable>().getItem2Check()) && colisio.collider.GetComponent<CuttingTable>().isPlateReady())
                {
                    GameObject item = colisio.collider.GetComponent<CuttingTable>().getItem();
                    carry.GetComponent<Olla>().setItem(item);
                }
                break;
            case "Papelera":
                if (carry.tag == "Olla")
                {
                    carry.GetComponent<Olla>().dumpContent();
                }
                else if(carry.tag == "Plate")
                {
                    carry.GetComponent<Plate>().dumpContent();
                }
                else if(carry.tag == "Sarten")
                {
                    carry.GetComponent<Sarten>().dumpContent();
                }
                else
                {
                    Debug.Log("Destruir comida");
                    Destroy(carry.gameObject);
                    carry = null;
                }
                break;
            case "Delivery": //Hacer funcion check
                if (carry.tag == "Plate" )
                {
                    carry.transform.parent = colisio.collider.transform;
                    colisio.collider.GetComponent<Delivery>().setItem(carry);
                    carry = null;
                }
                break;
        }
    }
    
    private void pickItem(RaycastHit colisio)
    {
        Debug.Log("Tried to pick item from " + colisio.collider.name); 
        switch (colisio.collider.tag)
        {
            case "Ingredient":
                colisio.collider.GetComponent<IngredientsBox>().AnimationOpen();
                GameObject pickup = Controller.getItem(colisio.collider.GetComponent<IngredientsBox>().getIngredientName());
                Vector3 itemPos = colisio.transform.position + new Vector3(0, transform.position.y + 1.3f, 0);
                carry = Instantiate(pickup, itemPos, rotateAngle, transform);
                //carry.transform.GetChild(2).GetComponent<Ingredients>().spawn_ingredient(pickup.tag);
                break;

            case "mesa":
                if (colisio.collider.GetComponent<Encimera>().hasItem()) carry = colisio.collider.GetComponent<Encimera>().getItem();
                break;

            case "SpawnPlates":
                //TODO
                if (colisio.collider.GetComponent<PlateSpawner>().hasItem()) carry = colisio.collider.GetComponent<PlateSpawner>().getItem();
                Debug.Log(carry.name);
                break;

            case "fogon":
                if (colisio.collider.GetComponent<Fogon>().hasItem() && !colisio.collider.GetComponent<Fogon>().onFire()) carry = colisio.collider.GetComponent<Fogon>().getItem();
                break;

            case "horno":
                if (colisio.collider.GetComponent<Encimera>().hasItem()) carry = colisio.collider.GetComponent<Encimera>().getItem();
                break;

            case "cuttingBoard":
                if (colisio.collider.GetComponent<CuttingTable>().hasItem() && colisio.collider.GetComponent<CuttingTable>().isCut())
                    carry = colisio.collider.GetComponent<CuttingTable>().getItem();
                break;
        }
        if(carry != null) {
            Debug.Log("YOU CARRY THINGS!");
            carry.transform.parent = transform;
            //carry.transform.position 
        }
    }
    private bool CheckObstacle()
    {
        //public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);
        Vector3 origin = transform.position;
        origin.x += raycastSeparation;
        return Physics.Raycast(origin, new Vector3(1, 0, 0), transform.position.x - chestPoint.transform.position.x, mask); //mask seran els obstacles que ens interessa que retornin true
    }   
    private bool placable(RaycastHit colisio)
    {
        switch (colisio.collider.tag)
        {
            case "mesa":
                if (!colisio.collider.GetComponent<Encimera>().hasItem())
                {
                    Debug.Log("Found Empty mesa");
                    return true;
                }
                else return false;
            case "plate":
                if (carry.tag != "plate" && !colisio.collider.GetComponent<Plate>().isFull()) return true;
                else return false;
            default:
                return false;
        }
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (!collision)
        {
            Vector3 movimiento = new Vector3(h, 0, v);
            body.MovePosition(transform.position + movimiento * Time.deltaTime * speed);
        }
    }
    private bool isIngredient(string tag)
    {
        switch (tag)
        {
            case "Cheese":
            case "Steak":
            case "Mushroom":
            case "Lettuce":
            case "Tomato":
            case "Dough":
            case "Sausage":
            case "Bread":
                return true;
            default: return false;
        }
    }

    private bool isContainer(string tag)
    {
        switch (tag)
        {
            case "Plate":
            case "Olla":
            case "Sarten":
                Debug.Log("Container");
                return true;
            default: return false;
        }
    }

    private bool isCompatible(GameObject container,GameObject ingredient)
    {
        if (container.tag == "Plate" && container.GetComponent<Plate>().has_Bread() && ingredient.tag == "Bread") return false;
        else if (container.tag == "Plate" && container.GetComponent<Plate>().has_Dough() && ingredient.tag == "Dough") return false;
        foreach (String Ing in compatibility[container.tag])
        {
            if (Ing == ingredient.tag) return true;
        }
        Debug.Log("NOT COMPATIBLE");
        return false;
    }
}

