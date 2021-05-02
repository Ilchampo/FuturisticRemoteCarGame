using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Classes Definition
// Class that contains information about the car axles
[System.Serializable]
public class AxleInformation
{
    public Transform leftWheel;
    public Transform rightWheel;
    public bool canTurn;
    public float maxWheelTurn = 25f;
}

// Class that contains the attributes for the motor
[System.Serializable]
public class CarMotor
{
    private float speedInput;
    private float turnInput;
    public float forwardAcceleration = 8f;
    public float reverseAcceleration = 4f;
    public float maxSpeed = 50f;
    public float turnStrenght = 180f;

    // Setters and Getter
    public void setSpeedInput(float speedInput) { this.speedInput = speedInput; }
    public float getSpeedInput() { return this.speedInput; }
    public void setTurnInput(float turnInput) { this.turnInput = turnInput; }
    public float getTurnInput() { return this.turnInput; }
}

// Class that contains the attributes used for physics
[System.Serializable]
public class CarPhysics
{
    private bool onGround = true;
    public Transform groundRayPoint;
    public LayerMask ground;
    public float gravityForce = 10f;
    public float dragToGround = 5f;
    public float groundRayLength = 0.5f;

    // Setters and Getters
    public void setOnGround(bool onGround) { this.onGround = onGround; }
    public bool getOnGround() { return this.onGround; }
}
#endregion

// Main CarController class
public class CarController : MonoBehaviour
{
    public Rigidbody carRigidBody;
    public List<AxleInformation> axleInformation;
    public CarMotor motor;
    public CarPhysics physicsAttributes;

    // Default attributes or methods when initialized 
    void Start()
    {
        // Detach rigid body from parent
        carRigidBody.transform.parent = null;
    }

    // Method that calls everyframe
    void Update()
    {
        motor.setSpeedInput(0);
        calculateSpeedInput();
        calculateTurnInput();
        wheelAnimation();
        transform.position = carRigidBody.transform.position;
    }

    #region Movement Attributes
    // Wheel rotation animation
    private void wheelAnimation()
    {
        foreach(AxleInformation axle in axleInformation)
        {
            if (!axle.canTurn)
                continue;
            axle.leftWheel.localRotation = Quaternion.Euler(
                axle.leftWheel.localRotation.eulerAngles.x,
                motor.getTurnInput() * axle.maxWheelTurn,
                axle.leftWheel.localRotation.eulerAngles.z);

            axle.rightWheel.localRotation = Quaternion.Euler(
                axle.rightWheel.localRotation.eulerAngles.x,
                motor.getTurnInput() * axle.maxWheelTurn,
                axle.rightWheel.localRotation.eulerAngles.z);
        }
    }
    // Moves car forward, checks if car is on air
    private void movesCarForward()
    {
        if(physicsAttributes.getOnGround())
        {
            carRigidBody.drag = physicsAttributes.dragToGround;
            if(Mathf.Abs(motor.getSpeedInput()) > 0)
                carRigidBody.AddForce(transform.forward * motor.getSpeedInput());
        } 
        else
        {
            carRigidBody.drag = 0.1f;
            carRigidBody.AddForce(Vector3.up * -physicsAttributes.gravityForce * 100f);
        }
    }
    #endregion

    #region Physics Methods 
    // Method to calculte the speed input
    private void calculateSpeedInput()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            motor.setSpeedInput(Input.GetAxis("Vertical") * motor.forwardAcceleration * 1000f);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            motor.setSpeedInput(Input.GetAxis("Vertical") * motor.reverseAcceleration * 1000f);
        }
    }
    // Method to calculate the turn input
    private void calculateTurnInput()
    {
        motor.setTurnInput(Input.GetAxis("Horizontal"));
        if(physicsAttributes.getOnGround())
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles +
                new Vector3(0f, motor.getTurnInput() * motor.turnStrenght * Time.deltaTime * Input.GetAxis("Vertical"), 0f)
            );
    }
    // Check if the player is on ground with raycast and calculates the relative rotation with the ground  
    private void checkIfOnGround()
    {
        RaycastHit raycastHit;
        if(Physics.Raycast(
            physicsAttributes.groundRayPoint.position,
            -transform.up, 
            out raycastHit,
            physicsAttributes.groundRayLength,
            physicsAttributes.ground
        ))
        {
            physicsAttributes.setOnGround(true);
            transform.rotation = Quaternion.FromToRotation(transform.up, raycastHit.normal) * transform.rotation;
        }
    }
    #endregion

    // Call methods related to physics here
    void FixedUpdate()
    {
        checkIfOnGround();
        movesCarForward();
    }
}
