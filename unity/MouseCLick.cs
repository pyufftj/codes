/*
 *此脚本的功能是解决UI穿透问题，当点击一个按钮时，可能跟着连后面的物体也点击了，触发了物体的事件。
 *解决UI方法是，点击一个地方时，判断穿透了几个物体，如果是一个，说明只有canvas一个，可以触发事件。
 *如果大于一个，则不触发事件。
 *穿透的所有物体都保存在raycastResult数组里。
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseCLick : MonoBehaviour {
	public EventSystem es;  //挂在eventsystem上
	public GraphicRaycaster grc;  //挂在主Canvas上。
	public List<RaycastResult> list;

	bool CheckHuiRayCastObjects(){
		PointerEventData eventData = new PointerEventData (es);
		eventData.pressPosition = Input.mousePosition;
		eventData.position = Input.mousePosition;

		list = new List<RaycastResult> ();
		grc.Raycast (eventData, list);
		for (int i = 0; i < list.Count; i++) {
			Debug.Log ("名字是：" + list [i].gameObject.name);

		}

		return list.Count > 0;
	}

	void Click(){
		Debug.Log (CheckHuiRayCastObjects());
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Click (); //如果点击鼠标左键，则触发了click事件。
		}
	}
}
