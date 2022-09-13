using UnityEngine;
using System.Collections;

public class InputManagerScript : MonoBehaviour 
{

	protected GameManagerScript gameManager;
	protected MoveTokensScript moveManager;
	protected GameObject selected = null;

	public virtual void Start() 
	{
		moveManager = GetComponent<MoveTokensScript>();
		gameManager = GetComponent<GameManagerScript>();
	}
		
	//for selecting token
	public virtual void SelectToken()
	{
		//if mouse click, get the position and convert to world position
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			//checks for mouse postion overlapping token collider
			Collider2D collider = Physics2D.OverlapPoint(mousePos);

			//if mouse is colliding
			if(collider != null)
			{
				//and nothing else is selected, select current token
				if(selected == null)
				{
					selected = collider.gameObject;
				} else //otherwise, stores position of selected object and the target object
				{
					Vector2 pos1 = gameManager.GetPositionOfTokenInGrid(selected);
					Vector2 pos2 = gameManager.GetPositionOfTokenInGrid(collider.gameObject);

					//checks for orthogonal tokens and sets up exchange manager

					//NEW BUG FIX!
					//Mathf.Abs was inititally giving the absolute value for the sum of pos1.x - pos2.x and pos1.y - pos2.y.
					//When changing it to giving absolute values of pos1.x - pos2.y and pos1.y - posr2.y seperately and later summing them solved the diagonal switching bug
					if((Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 1)
					{
						print("EXCHANGE TOKEN");
						moveManager.SetupTokenExchange(selected, pos1, collider.gameObject, pos2, true);
					}
					//sets the selected token to null
					selected = null;
				}
			}
		}

	}

}
