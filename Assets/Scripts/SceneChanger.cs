using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
	#region UI GameObjects

	public Animator Animator;

	#endregion
	
	#region Instance

	private static SceneChanger instance;

	public static SceneChanger InstanceSC
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<SceneChanger>();
			}

			return SceneChanger.instance;
		}
		
	}

	#endregion
	public void backBtn()
	{
		StartCoroutine(Transition(0));
	}

	/*public void retryBtn()
	{
		StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex));
	}*/
	
	public void forwardBtn()
	{
		StartCoroutine(Transition(0));
	}

	public IEnumerator Transition(int LevelIndex)
	{
		Animator.SetTrigger("Start");
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(LevelIndex);
	}
}
