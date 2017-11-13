using UnityEngine;
using System.Collections;

[AddComponentMenu("Prairie/Interactions/Animate Component")]
public class ComponentToggleAnimation : PromptInteraction
{
	public GameObject[] targets = new GameObject[0];
	public int animX = 0;
	public int animY = 0;
	public int animZ = 0;
	public int speed = 0;

	void OnDrawGizmosSelected()
	{
		// sets line specifications
		Gizmos.color = Color.red;
		for (int i = 0; i < targets.Length; i++)
		{
			// Draw red line(s) between the object and the objects whose Behaviours it toggles
			if (targets[i] != null)
			{
				Gizmos.DrawLine(transform.position, targets[i].transform.position);
			}

		}
	}

	// for all attached targets, when the object with this component attached is
	// clicked, switch the enable boolean of each target
	// turns behaviors on/off for light switches
	protected override void PerformAction ()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			Vector3 startPos = targets[i].transform.position;
			Vector3 destPos = startPos;
			destPos[0] += animX;
			destPos[1] += animY;
			destPos[2] += animZ;
			float step = speed * Time.deltaTime;
			targets[i].transform.position = Vector3.MoveTowards(startPos, destPos, step);
		}
	}

	override public string defaultPrompt {
		get {
			return "Animate Something";
		}
	}
}
