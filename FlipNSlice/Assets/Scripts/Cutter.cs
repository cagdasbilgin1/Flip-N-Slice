using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Cutter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (other.gameObject.CompareTag("Cuttable"))
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            other.enabled = false;
            GameObject piece1 = other.gameObject.transform.GetChild(0).gameObject;
            GameObject piece2 = other.gameObject.transform.GetChild(1).gameObject;
            Game.money += 1;
            gm.money.text = "$" + Game.money.ToString();

            Rigidbody rb1 = piece1.GetComponent<Rigidbody>();
            rb1.isKinematic = false;
            rb1.constraints = RigidbodyConstraints.None;
            rb1.AddForce(new Vector3(200, 0, 0));
            Destroy(piece1.gameObject, 2);
        }
        else if (other.gameObject.CompareTag("CuttableDouble"))
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            other.enabled = false;

            GameObject piece1 = other.gameObject.transform.GetChild(0).gameObject;
            GameObject piece2 = other.gameObject.transform.GetChild(1).gameObject;
            Game.money += 1;
            gm.money.text = "$" + Game.money.ToString();

            Rigidbody rb1 = piece1.GetComponent<Rigidbody>();
            Rigidbody rb2 = piece2.GetComponent<Rigidbody>();
            rb1.constraints = RigidbodyConstraints.None;
            rb2.constraints = RigidbodyConstraints.None;
            rb1.AddForce(new Vector3(300, 0, 0));
            rb2.AddForce(new Vector3(-300, 0, 0));
            Destroy(other.gameObject, 2);
        }
        else if (other.gameObject.CompareTag("ScoreBoard"))
        {
            other.gameObject.GetComponent<AudioSource>().Play();
            other.gameObject.GetComponent<Animator>().enabled = true;
            
            //So that the knife does not hit the scoreboard more than once
            Collider[] colliders;
            colliders = other.gameObject.transform.parent.GetComponentsInChildren<Collider>();
            foreach(Collider c in colliders)
            {
                c.enabled = false;
            }


            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.GetComponent<Animator>().enabled = false;

            string scorePoint = other.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
            scorePoint = scorePoint.Substring(0, scorePoint.Length - 1);
            int scoreNumber = Int32.Parse(scorePoint);

            int previousLevelMoney = PlayerPrefs.GetInt(gm.GetPreviousLevel());

            Game.money -= previousLevelMoney;
            Game.money *= scoreNumber;
            Game.money += previousLevelMoney;

            gm.money.text = "$" + Game.money.ToString();

            PlayerPrefs.SetInt(gm.GetCurrentLevel(), Game.money);
            StartCoroutine(NextLevel());           

        }

        IEnumerator NextLevel()
        {            
            gm.Win();
            yield return new WaitForSeconds(3);
            gm.NextLevel();
        }
    }
}
