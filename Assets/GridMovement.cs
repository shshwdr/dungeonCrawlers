using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    Vector3 up = Vector3.zero;
    Vector3 right = new Vector3(0,90,0);
    Vector3 down = new Vector3(0, 180, 0);
    Vector3 left = new Vector3(0, 270, 0);
    public Transform foot;
    Vector3 currentDirection = Vector3.zero;
    Vector3 nextPos, destination, direction;
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float moveStep = 1f;
    bool isMoving;
    BattlePlayer player;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BattlePlayer>();
        Reset();
    }

    public void Reset()
    {

        currentDirection = up;
        nextPos = Vector3.forward;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleSystem.Instance.isInBattle || ShopManager.Instance.isInShop || player.isDead || CSDialogueManager.Instance.isInDialogue)
        {
            return;
        }
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector3.Distance(destination, transform.position) <= 0.001f)
            {
                isMoving = false;
                LayerMask layer = 1 << LayerMask.NameToLayer("DangerZone");
                //check if need to trigger battle

                var overlaps = Physics.OverlapSphere(transform.position, 0.1f, layer);
                if(overlaps.Length == 1)
                {
                    Transform playerTransform = player.transform;
                    Vector3 playerPosition = playerTransform.position;
                    Vector3 playerForward = playerTransform.forward * moveStep;
                    Quaternion monsterRotation = Quaternion.LookRotation(-playerForward);
                    Vector3 monsterPosition = playerPosition + playerForward;
                    bool canTriggerBattle = ValidSpawn(playerPosition, playerForward);
                    if (ZoneManager.Instance.moveInDangerZone(canTriggerBattle))
                    {
                        string zoneId = overlaps[0].GetComponent<DangerZone>().id;


                        //check if front is wall or monster
                            ZoneManager.Instance.StartPopupBattle(zoneId, player, monsterPosition, monsterRotation);


                    }
                }
                else if (overlaps.Length > 1)
                {
                    Debug.LogError("overlap with two danger zone!");
                }

            }
            return;
        }

        if (Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            if (GetIntoBattle(transform.forward * moveStep))
            {
                return;
            }
            if (GetIntoShop(transform.forward * moveStep))
            {
                return;
            }
            if (GetIntoInteractable(transform.forward * moveStep))
            {
                return;
            }
            
            if (ValidMove(transform.forward * moveStep))
            {
                nextPos = transform.forward * moveStep;
                isMoving = true;
                destination = transform.position + nextPos;
            }

        }
        else if (Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            //if(GetIntoBattle(-transform.forward * moveStep))
            //{
            //    return;
            //}
            //if (ValidMove(-transform.forward * moveStep))
            //{
            //    nextPos = -transform.forward * moveStep;
            //    isMoving = true;
            //    destination = transform.position + nextPos;
            //}

            currentDirection += left*2;

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentDirection += left;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentDirection += right; 
           
        }
        transform.localEulerAngles = currentDirection;
    }

    bool ValidMove(Vector3 direction)
    {
        Ray myRay = new Ray(transform.position, direction * moveStep);
        LayerMask layerMask = LayerMask.GetMask("Wall");
        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);
        if (Physics.Raycast(myRay, moveStep, layerMask))
        {
            return false;
        }
        return true;
    }

    bool ValidSpawn(Vector3 position, Vector3 direction)
    {
        //todo this does not work
        Ray myRay = new Ray(position, direction * moveStep);
        LayerMask wallMask = LayerMask.GetMask("Wall");

        LayerMask monsterMask = LayerMask.GetMask("Monster");
        LayerMask shopMask = LayerMask.GetMask("Shop");
        LayerMask interactableMask = LayerMask.GetMask("Interactable");
        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);
        if (Physics.Raycast(myRay, moveStep, shopMask))
        {
            return false;
        }
        if (Physics.Raycast(myRay, moveStep, monsterMask))
        {
            return false;
        }
        if (Physics.Raycast(myRay, moveStep, interactableMask))
        {
            return false;
        }
        if (Physics.Raycast(myRay, moveStep, wallMask))
        {
            return false;
        }
        return true;
    }

    bool GetIntoBattle(Vector3 direction)
    {
        Ray myRay = new Ray(foot.position, direction * moveStep);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Monster");
        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);
        if (Physics.Raycast(myRay, out hit, moveStep, layerMask))
        {
            var monster = hit.collider.GetComponent<Monster>();
            if (monster)
            {
                BattleSystem.Instance.StartBattle(monster, player);
                return true;
            }
        }
        return false;
    }

    bool GetIntoLayer(Vector3 direction, string layerName, out RaycastHit hit)
    {
        Ray myRay = new Ray(foot.position, direction * moveStep);
        LayerMask layerMask = LayerMask.GetMask(layerName);
        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);
        if (Physics.Raycast(myRay, out hit, moveStep, layerMask))
        {
            return true;
        }
        return false;
    }

    bool GetIntoShop(Vector3 direction)
    {
        RaycastHit hit;
        var res = GetIntoLayer(direction, "Shop",out hit);
        if (res)
        {
            ShopManager.Instance.showShopMenu();
        }
        return res;
    }

    bool GetIntoInteractable(Vector3 direction)
    {
        RaycastHit hit;
        var res = GetIntoLayer(direction, "Interactable",out hit);
        if (res)
        {
            var interactable = hit.collider.GetComponent<Interactable>();
            interactable.Interact();
        }
        return res;
    }
}
