using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator animator;

    PlayerStats stats;

    Inventory inventory;

    public Player playerManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.transform.GetComponent<Animator>();
        stats = this.transform.root.GetComponent<PlayerStats>();
        inventory = transform.root.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.equippedWeapon == WeaponTypes.NoWeapon) // no weapon animations
        {
            animator.SetInteger("treeSelector", 1);

            if (playerManager.isGrounded)
            {
                if (playerManager.keysPressed[4]) //jump
                {
                    animator.Play("Jump");
                }
                else if (playerManager.keysPressed[0] && playerManager.keysPressed[5]) //sprint
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 2, 1f, Time.deltaTime * 10f);
                    animator.speed = 1f;
                }
                else if (playerManager.keysPressed[0]) //walk forward
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[2]) //walk backwards
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", -1, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[3]) //walk right
                {
                    animator.SetFloat("xCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[1]) //walk left
                {
                    animator.SetFloat("xCoordRifle", -1, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.2f;
                }
                else //idle
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1f;
                }
            }
        } 
        else if (stats.equippedWeapon == WeaponTypes.Revolver) //pistol animations
        {
            animator.SetInteger("treeSelector", 2);

            if (playerManager.isGrounded)
            {
                if (playerManager.keysPressed[4]) //jump
                {
                    animator.Play("Jump");
                }

                if (playerManager.keysPressed[0] && playerManager.keysPressed[5]) //sprint
                {
                    animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordPistol", 2, 1f, Time.deltaTime * 10f);
                    animator.speed = 1f;
                }
                else if (playerManager.keysPressed[0]) //walk forward
                {
                    animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordPistol", 1, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[2]) //walk backwards
                {
                    animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordPistol", -1, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[3]) //walk right
                {
                    animator.SetFloat("xCoordPistol", 1, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[1]) //walk left
                {
                    animator.SetFloat("xCoordPistol", -1, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.2f;
                }
                else //idle
                {
                    animator.SetFloat("xCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordPistol", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1f;
                }
            }
            else
            {
                if (playerManager.keysPressed[6]) //fire
                {
                    animator.Play("FirePistol");
                }
                else if (playerManager.keysPressed[7]) //aim
                {
                    //animator.Play("AimRifle");
                }
            }
        } 
        else
        {
            animator.SetInteger("treeSelector", 1);

            if (playerManager.isGrounded)
            {
                if (playerManager.keysPressed[4]) //jump
                {
                    animator.Play("Jump");
                }
                else if (playerManager.keysPressed[6]) //fire
                {
                    animator.Play("FireRifle");
                }
                else if (playerManager.keysPressed[7]) //aim
                {
                    animator.Play("AimRifle");
                }

                if (playerManager.keysPressed[0] && playerManager.keysPressed[5]) //sprint
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 2, 1f, Time.deltaTime * 10f);
                    animator.speed = 1f;
                }
                else if (playerManager.keysPressed[0]) //walk forward
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[2]) //walk backwards
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", -1, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[3]) //walk right
                {
                    animator.SetFloat("xCoordRifle", 1, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.4f;
                }
                else if (playerManager.keysPressed[1]) //walk left
                {
                    animator.SetFloat("xCoordRifle", -1, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1.2f;
                }
                else //idle
                {
                    animator.SetFloat("xCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.SetFloat("yCoordRifle", 0, 1f, Time.deltaTime * 10f);
                    animator.speed = 1f;
                }
            }
            else
            {
                if (playerManager.keysPressed[6]) //fire
                {
                    animator.Play("FireRifle");
                }
                else if (playerManager.keysPressed[7]) //aim
                {
                    animator.Play("AimRifle");
                }
            }
        }
    }

    public Transform RecursiveFindChild(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == tag)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, tag);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(2.5f);
    }
}
