using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//git versionierung + alex link schicken

public class PlayerBehavior : MonoBehaviour {  

	[SerializeField]
	GameObject player;

	[SerializeField]
	GameObject objective;

	[SerializeField]
	GameObject danger;

	[SerializeField]
	float angle;

	[SerializeField]
	float speed;

	[SerializeField]
	float[] interestMap;

	[SerializeField]
	float[] dangerMap;

	[SerializeField]
	float[] result;

	[SerializeField]
	float[,] valueMatrix;

	Sensors data;

	CircleCollider2D cc;

	public List<GameObject> interestObjects;
	public List<GameObject> dangerObjects;

	public struct Sensor
    {
		Vector3 position;
		public string name;
		public Vector3 direction;

		public Sensor(float x, float y, string sensorName, Vector3 sensorDirection)
        {
			this.position = new Vector3(x, y, 0);
			this.name = sensorName;
			this.direction = sensorDirection;
        }

		public void SetPosition(float x, float y)
        {
			position.x = x;
			position.y = y;
        }

		public Vector3 GetPosition()
        {
			return position;
        }
    }

	public struct Sensors
	{
		public Sensor[] sensors;
		public float range;

		public Sensors(float posX, float posY, float range)
        {
			Sensor s1 = new Sensor(posX, posY + 0.25f, "s1", new Vector3(0, 1, 0));
			Sensor s2 = new Sensor(posX + 0.25f, posY + 0.25f, "s2", new Vector3(1, 1, 0));
			Sensor s3 = new Sensor(posX + 0.25f, posY, "s3", new Vector3(1, 0, 0));
			Sensor s4 = new Sensor(posX + 0.25f, posY - 0.25f, "s4", new Vector3(1, -1, 0));
			Sensor s5 = new Sensor(posX, posY - 0.25f, "s5", new Vector3(0, -1, 0));
			Sensor s6 = new Sensor(posX - 0.25f, posY - 0.25f, "s6", new Vector3(-1, -1, 0));
			Sensor s7 = new Sensor(posX - 0.25f, posY, "s7", new Vector3(-1, 0, 0));
			Sensor s8 = new Sensor(posX - 0.25f, posY + 0.25f, "s8", new Vector3(-1, 1, 0));

			this.range = range;

			List<Sensor> temp = new List<Sensor>();

			temp.Add(s1);
			temp.Add(s2);
			temp.Add(s3);
			temp.Add(s4);
			temp.Add(s5);
			temp.Add(s6);
			temp.Add(s7);
			temp.Add(s8);

			sensors = temp.ToArray();
		}

		public void SensorUpdate(float posX, float posY)
		{
			for(int i = 0; i < sensors.Length; i += 1)
            {
				switch(sensors[i].name)
                {
					case "s1": sensors[i] = new Sensor(posX,		 posY + 0.25f, "s1", new Vector3(0, 1, 0)); break;
					case "s2": sensors[i] = new Sensor(posX + 0.25f, posY + 0.25f, "s2", new Vector3(1, 1, 0)); break;
					case "s3": sensors[i] = new Sensor(posX + 0.25f, posY,		   "s3", new Vector3(1, 0, 0)); break;
					case "s4": sensors[i] = new Sensor(posX + 0.25f, posY - 0.25f, "s4", new Vector3(1, -1, 0)); break;
					case "s5": sensors[i] = new Sensor(posX,		 posY - 0.25f, "s5", new Vector3(0, -1, 0)); break;
					case "s6": sensors[i] = new Sensor(posX - 0.25f, posY - 0.25f, "s6", new Vector3(-1, -1, 0)); break;
					case "s7": sensors[i] = new Sensor(posX - 0.25f, posY,		   "s7", new Vector3(-1, 0, 0)); break;
					case "s8": sensors[i] = new Sensor(posX - 0.25f, posY + 0.25f, "s8", new Vector3(-1, 1, 0)); break;
				}
			}
        }
	}


	// Use this for initialization
	void Start ()
	{
		cc = player.GetComponent<CircleCollider2D>();
		data = new Sensors(player.transform.position.x, player.transform.position.y, cc.radius);
		interestObjects = new List<GameObject>();
		dangerObjects = new List<GameObject>();
		speed = 0.5f;
	}

	// Update is called once per frame
	void Update ()
	{

        //float changeX = 0;
        //float changeY = 0;

        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    changeY = 1f;
        //}
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    changeY = -1f;
        //}
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    changeX = -1f;
        //}
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    changeX = 1f;
        //}

        interestMap = new float[8];
		dangerMap = new float[8];
		result = new float[8];

		interestMap = ObjectiveContextMapGeneration(angle, interestObjects, data.sensors);
		dangerMap = ObjectiveContextMapGeneration(angle, dangerObjects, data.sensors);

		result = CombiningMaps(interestMap, dangerMap);

		float max = 0;
		int index = -1;

		for(int i = 0; i < result.Length; i += 1)
        {
			if(result[i] > max)
            {
				max = result[i];
				index = i;
            }
        }


		//default random movement unitl next update
		Vector3 changeDirection = new Vector3(0, 0, 0);

		switch(index)
        {
			case 0: changeDirection = new Vector3(0, 1, 0); break;
			case 1: changeDirection = new Vector3(1, 1, 0); break;
			case 2: changeDirection = new Vector3(1, 0, 0); break;
			case 3: changeDirection = new Vector3(1, -1, 0); break;
			case 4: changeDirection = new Vector3(0, -1, 0); break;
			case 5: changeDirection = new Vector3(-1, -1, 0); break;
			case 6: changeDirection = new Vector3(-1, 0, 0); break;
			case 7: changeDirection = new Vector3(-1, 1, 0); break;
		}

		Vector3 newPosi = player.transform.position + changeDirection * speed * Time.deltaTime;
		//Vector3 newPosi = player.transform.position + new Vector3(changeX, changeY, 0).normalized * speed * Time.deltaTime;

		player.transform.position = newPosi;

		data.SensorUpdate(newPosi.x, newPosi.y);
	}


	public float[] ObjectiveContextMapGeneration(float angle, List<GameObject> referencePoints, Sensor[] data)
    {
		float[] contextMap = new float[8];

		for(int i = 0; i < data.Length; i += 1)
        {
			float result = -1;

			foreach(GameObject o in referencePoints)
            {
				if(o == null)
                {
					continue;
                }

				float temp = -1;
				Vector3 receptorDirection = data[i].direction;
				Vector3 steeringDirection = o.transform.position - data[i].GetPosition();

				float resultingAngle = Mathf.Acos(DotProduct(receptorDirection, steeringDirection) / (MagnitudeVector(receptorDirection) * MagnitudeVector(steeringDirection)));
				resultingAngle = (resultingAngle / (2 * Mathf.PI)) * 360;

                if (resultingAngle <= angle)
				{
					temp = AngleMapping(resultingAngle, angle) * DistanceMapping(MagnitudeVector(steeringDirection), data.Length);
				}

				if (temp > result)
				{
					result = temp;
				}
			}

			if(result < 0)
            {
				result = 0;
            }

			contextMap[i] = result;
		}

		return contextMap;
    }

	public float[] CombiningMaps(float[] interest, float[] danger)
    {
		float[] result = new float[8];

		for(int i = 0; i < result.Length; i+= 1)
        {
			if(interest[i] > danger[i])
            {
				result[i] = interest[i] - danger[i] * 0.9f;
            }
			else
            {
				result[i] = 0;
            }
        }
		return result;
    }

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Interest")
		{
			interestObjects.Add(collider.gameObject);
		}
		if (collider.gameObject.tag == "Danger")
		{
			dangerObjects.Add(collider.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Interest")
		{
			interestObjects.Remove(collider.gameObject);
		}
		if (collider.gameObject.tag == "Interest")
		{
			dangerObjects.Remove(collider.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
		Destroy(collision.gameObject);
		interestObjects.Remove(collision.gameObject);
	}

	float DotProduct (Vector3 input1, Vector3 input2)
    {
		return input1.x * input2.x + input1.y * input2.y + input1.z + input2.z;
    }

	float MagnitudeVector(Vector3 input)
	{
		return Mathf.Sqrt(input.x * input.x + input.y * input.y + input.z * input.z);
	}

	float AngleMapping(float inputAngle, float threshold)
    {
		return 1 - (inputAngle / (threshold / 2));
    }

	float DistanceMapping(float distance, float sensorRange)
    {
		return 1 - (distance / sensorRange);
    }

	void MatrixSteering(List<GameObject> interestObjects, List<GameObject> dangerObjects, Sensor[] data, float angle)
    {

		//calculation of the whole interest matrix sensor x interestObjects
		valueMatrix = new float[data.Length, interestObjects.Count];

		for(int i = 0; i < data.Length; i += 1)
        {
			for(int j = 0; j < interestObjects.Count; j += 1)
			{
				if (interestObjects[j] == null)
				{
					continue;
				}

				float temp = -1;
				Vector3 receptorDirection = data[i].direction;
				Vector3 steeringDirection = interestObjects[j].transform.position - data[i].GetPosition();

				float resultingAngle = Mathf.Acos(DotProduct(receptorDirection, steeringDirection) / (MagnitudeVector(receptorDirection) * MagnitudeVector(steeringDirection)));
				resultingAngle = (resultingAngle / (2 * Mathf.PI)) * 360;

				if (resultingAngle <= angle)
				{
					temp = AngleMapping(resultingAngle, angle) * DistanceMapping(MagnitudeVector(steeringDirection), data.Length);
				}

				if(temp < 0)
                {
					temp = 0;
                }

				valueMatrix[i,j] = temp;
            }
        }


		//calculation of the maximal danger value for each sensor
		float[] dangerMap = new float[8];

		for (int i = 0; i < data.Length; i += 1)
		{
			float result = -1;

			foreach (GameObject o in dangerObjects)
			{
				if (o == null)
				{
					continue;
				}

				float temp = -1;
				Vector3 receptorDirection = data[i].direction;
				Vector3 steeringDirection = o.transform.position - data[i].GetPosition();

				float resultingAngle = Mathf.Acos(DotProduct(receptorDirection, steeringDirection) / (MagnitudeVector(receptorDirection) * MagnitudeVector(steeringDirection)));
				resultingAngle = (resultingAngle / (2 * Mathf.PI)) * 360;

				if (resultingAngle <= angle)
				{
					temp = AngleMapping(resultingAngle, angle) * DistanceMapping(MagnitudeVector(steeringDirection), data.Length);
				}

				if (temp > result)
				{
					result = temp;
				}
			}

			if (result < 0)
			{
				result = 0;
			}

			dangerMap[i] = result;
		}

		//combining interestValues with maximal danger value for each sensor
		for (int i = 0; i < data.Length; i += 1)
		{
			for (int j = 0; j < interestObjects.Count; j += 1)
			{
				if (interestObjects[j] == null)
				{
					continue;
				}

				if (valueMatrix[i,j] > dangerMap[i])
				{
					valueMatrix[i,j] -= dangerMap[i] * 0.9f;
				}
				else
				{
					valueMatrix[i,j] = 0;
				}
			}
		}


		//conclusion of direction
	}
}
