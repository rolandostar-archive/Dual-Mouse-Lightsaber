using System;
using UnityEngine;
using System.Collections;
using DigitalRuby.LaserSword;

public class _MouseInput_1Object : MonoBehaviour {
	public GameObject obj; // Objectos a Manipular
	public GameObject frame; // Objectos a Manipular
	public LaserSwordScript sword;

	public int indexLeft = 1;
	public int indexRight = 2;

	private ManyMouse[] _manyMouseMice; 	// Arreglo de Mouses
	private Vector3[] move; 			   // Vector a ser Aplicado a los objetos
	private Vector2[] moveUI; 			  // Vector a Mostrar en Interfaz

	private float actualDistance; // Distancia de la camara a los objetos
	public float sensLeft = 0.05f;   // Sensibilidad de Mouse
	public float sensRight = 0.05f;   // Sensibilidad de Mouse

	void Start() { // Inicializacion
		// Bloquear movimientos del mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (false);

		// Obtener Mouses conectados
		int numMice = ManyMouseWrapper.MouseCount;
		if (numMice > 0) {
			// Inicializar arreglos en base a nuemro de mouses
			_manyMouseMice = new ManyMouse[numMice];
			move = new Vector3[numMice];
			moveUI = new Vector2[numMice];
			// Obtener IDs de Mouses
			for (int i = 0; i < numMice; i++) {
				_manyMouseMice[i] = ManyMouseWrapper.GetMouseByID(i);
				_manyMouseMice[i].EventButtonDown += ActivateButton;
			}
		}

		// Obtener Distancia de Camara
		Vector3 toObjectVector = transform.position - Camera.main.transform.position;
		Vector3 linearDistanceVector = Vector3.Project(toObjectVector,Camera.main.transform.forward);
		actualDistance = linearDistanceVector.magnitude;
	} // Start

	private void ActivateButton(ManyMouse mouse, int buttonId){
		bool[] buttons = mouse.MouseButtons;
		Debug.Log (""+buttons[0] + buttons[1] + buttons[2] + buttons[3] + buttons[4]);
		if (buttons [1])
			ToggleSword ();
		//while(buttons[0])
		//	obj.transform.Rotate(new Vector3(0,Time.deltaTime * 1000,0));
		//sword.TurnOn(true);

	}

	private void ToggleSword() {if (!sword.TurnOn (true)) sword.TurnOn (false);}
		
	void Update() {
		//obj.transform.Rotate(new Vector3(0,0,Time.deltaTime * 500));
		// Re-Bloquear Mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (false);

		// Obtener 4 Axis
		Vector3 delta_l = _manyMouseMice[indexLeft].Delta;
		Vector3 delta_r = _manyMouseMice[indexRight].Delta;
		Vector3 pos_l = _manyMouseMice[indexLeft].Pos;
		Vector3 pos_r = _manyMouseMice[indexRight].Pos;

		float X1 = delta_l.x * sensLeft;
		float Y1 = delta_l.y * sensLeft;
		float X2 = delta_r.x * sensRight;
		float Y2 = delta_r.y * sensRight;

		if (moveUI[indexLeft].y < -3  && delta_l.y < 0) Y1 = 0;
		if (moveUI[indexLeft].y >  0.1  && delta_l.y > 0) Y1 = 0;
		if (moveUI[indexLeft].x < -3 && delta_l.x < 0) X1 = 0;
		if (moveUI[indexLeft].x >  3 && delta_l.x > 0) X1 = 0;

		//if (Y2 != 0 || X2 != 0) obj.transform.Translate(new Vector3(X2,Y2,actualDistance));
		if (X1 != 0) frame.transform.Translate(Vector3.right * X1);
		if (Y1 != 0) frame.transform.Translate(Vector3.down * Y1);
		moveUI[indexLeft] += new Vector2(X1,Y1); // Cumulativo

		if (moveUI[indexRight].y < -130  && delta_r.y < 0) Y2 = 0;
		if (moveUI[indexRight].y >  55  && delta_r.y > 0) Y2 = 0;
		if (moveUI[indexRight].x < -100  && delta_r.x < 0) X2 = 0;
		if (moveUI[indexRight].x >  100  && delta_r.x > 0) X2 = 0;

		if (X2 != 0) obj.transform.Rotate(new Vector3(0,0,-X2));
		if (Y2 != 0) frame.transform.Rotate(new Vector3(-Y2,0,0));
		moveUI[indexRight] += new Vector2(X2,Y2); // Cumulativo

		/*
		// Movimiento no-cumulativo
			Vector3 delta = _manyMouseMice[i].Delta;
			float X = delta.x * sens;
			float Y = delta.y * sens;
			// Checa limites (4 lados)
			if (moveUI[i].y < -5  && delta.y < 0) Y = 0;
			if (moveUI[i].y >  5  && delta.y > 0) Y = 0;
			if (moveUI[i].x < -10 && delta.x < 0) X = 0;
			if (moveUI[i].x >  10 && delta.x > 0) X = 0;
			moveUI[i] += new Vector2(X,Y); // Cumulativo
			move[i] = new Vector3(X,Y,actualDistance);
			// Aplica movimientos
			//if (move[i].y != 0)	objs[i].transform.Translate(Vector3.down * move[i].y);
			//if (move[i].x != 0)	objs[i].transform.Translate(Vector3.right * move[i].x);
			// Reset (No cumulativo)
			move[i].y = 0.0f;
			move[i].x = 0.0f;
		*/
		X2 = 0.0f;
		Y2 = 0.0f;
		X1 = 0.0f;
		Y1 = 0.0f;
	}

	// Mostrar Informacion
	void OnGUI(){
		GUILayout.Label("DEBUG:");
		for(int i=0; i< _manyMouseMice.Length; i++){
			bool[] buttons = _manyMouseMice[i].MouseButtons;
			if(_manyMouseMice[i] != null)
				GUILayout.Label("Mouse[" + i.ToString() + "] : " + moveUI[i] + buttons[0] + buttons[1] + buttons[2] + buttons[3] + buttons[4]);
		}
	}

}