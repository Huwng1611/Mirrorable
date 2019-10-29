using UnityEngine;

public class PigController : MonoBehaviour
{
    public Animator pigAnim;
    public Transform cam;
    public Rigidbody2D pigRigid;
    /// <summary>
    /// tọa độ x của pig
    /// </summary>
    public float xPos;
    /// <summary>
    /// tọa độ y của pig
    /// </summary>
    public float yPos;

    private void Start()
    {
        xPos = cam.position.x - transform.position.x + 3.5f;
        yPos = cam.position.y - transform.position.y + 2f;
    }

    /// <summary>
    /// sử dụng LateUpdate thay vì Update để con lợn di chuyển ko bị giật
    /// </summary>
    private void LateUpdate()
    {
        if (!EnemyManager.enemymanager.checkFight)
        {
            if (!GamePlay.gameplay.isGameOver)
            {
                if (PlayerPrefs.GetInt("levelBigTroop") >= 4)
                {
                    pigAnim.Play("Pigfly");
                    transform.position = new Vector3(cam.position.x - xPos, yPos, transform.position.z);
                    pigRigid.constraints = RigidbodyConstraints2D.FreezePositionY;
                    pigRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    pigAnim.Play("Pigrun");
                    transform.position = new Vector3(cam.position.x - xPos, transform.position.y, transform.position.z);
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("levelBigTroop") >= 4)
                {
                    pigAnim.Play("Pigfly");
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * -GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                    pigRigid.constraints = RigidbodyConstraints2D.FreezePositionY;
                    pigRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    pigAnim.Play("Pigrun");
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * -GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                }
            }
        }
        else if (EnemyManager.enemymanager.checkFight)
        {
            if (!GamePlay.gameplay.isGameOver)
            {
                pigAnim.Play("Pigidle");
                transform.position = new Vector3(cam.position.x - xPos, transform.position.y, transform.position.z);
                pigRigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            if (GamePlay.gameplay.isGameOver)
            {
                if (PlayerPrefs.GetInt("levelBigTroop") >= 4)
                {
                    pigAnim.Play("Pigfly");
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * -GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                }
                else
                {
                    pigAnim.Play("Pigrun");
                    transform.position = new Vector3(transform.position.x + Time.deltaTime * -GamePlay.gameplay.speed, transform.position.y, transform.position.z);
                }
            }
        }
    }
}
