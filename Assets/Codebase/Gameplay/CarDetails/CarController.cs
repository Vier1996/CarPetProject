using Codebase.Gameplay.CarDetails.Config;
using UnityEngine;

namespace Codebase.Gameplay.CarDetails
{
  public class CarController : MonoBehaviour, ICar
  {
    [SerializeField] private CarConfig _debugConfig;
    
    [Space(10)] 
    public Vector3 bodyMassCenter;

    public GameObject frontLeftMesh;
    public WheelCollider frontLeftCollider;
    [Space(10)] public GameObject frontRightMesh;
    public WheelCollider frontRightCollider;
    [Space(10)] public GameObject rearLeftMesh;
    public WheelCollider rearLeftCollider;
    [Space(10)] public GameObject rearRightMesh;
    public WheelCollider rearRightCollider;


    public ParticleSystem RLWParticleSystem;
    public ParticleSystem RRWParticleSystem;

    [Space(10)] 
    public TrailRenderer RLWTireSkid;
    public TrailRenderer RRWTireSkid;
    
    public AudioSource carEngineSound;
    public AudioSource tireScreechSound;
    float initialCarEngineSoundPitch;

    private CarConfig _currentConfig;
    private Rigidbody carRigidbody;

    private float carSpeed;
    private float steeringAxis;
    private float throttleAxis;
    private float driftingAxis;
    private float localVelocityZ;
    private float localVelocityX;
    private bool deceleratingCar;
    private bool isDrifting;
    private bool isTractionLocked;
    
    private WheelFrictionCurve FLwheelFriction;
    private WheelFrictionCurve FRwheelFriction;
    private WheelFrictionCurve RLwheelFriction;
    private WheelFrictionCurve RRwheelFriction;
    
    private float FLWextremumSlip;
    private float FRWextremumSlip;
    private float RLWextremumSlip;
    private float RRWextremumSlip;

    private void Start()
    {
      _currentConfig = _debugConfig;
      
      carRigidbody = gameObject.GetComponent<Rigidbody>();
      carRigidbody.centerOfMass = bodyMassCenter;

      FLwheelFriction = FRwheelFriction = RLwheelFriction = RRwheelFriction = new WheelFrictionCurve();
      
      FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
      FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
      RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
      RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;

      FLwheelFriction = SetupFriction(FLwheelFriction, frontLeftCollider);
      FRwheelFriction = SetupFriction(FLwheelFriction, frontRightCollider);
      RLwheelFriction = SetupFriction(FLwheelFriction, rearLeftCollider);
      RRwheelFriction = SetupFriction(FLwheelFriction, rearRightCollider);
      
      WheelFrictionCurve SetupFriction(WheelFrictionCurve targetCurve, WheelCollider wheelCollider)
      {
        WheelFrictionCurve curve = wheelCollider.sidewaysFriction;
        
        targetCurve.extremumSlip = curve.extremumSlip;
        targetCurve.extremumValue = curve.extremumValue;
        targetCurve.asymptoteSlip = curve.asymptoteSlip;
        targetCurve.asymptoteValue = curve.asymptoteValue;
        targetCurve.stiffness = curve.stiffness;

        return targetCurve;
      }
      
      if (carEngineSound != null) 
        initialCarEngineSoundPitch = carEngineSound.pitch;

      InvokeRepeating(nameof(CarSounds), 0f, 0.1f);
    }

    private void Update()
    {
      carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
      localVelocityX = transform.InverseTransformDirection(carRigidbody.velocity).x;
      localVelocityZ = transform.InverseTransformDirection(carRigidbody.velocity).z;

        if (Input.GetKey(KeyCode.W))
        {
          CancelInvoke(nameof(DecelerateCar));
          deceleratingCar = false;
          MoveUp();
        }

        if (Input.GetKey(KeyCode.S))
        {
          CancelInvoke(nameof(DecelerateCar));
          deceleratingCar = false;
          MoveDown();
        }

        if (Input.GetKey(KeyCode.A))
        {
          TurnLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
          TurnRight();
        }

        if (Input.GetKey(KeyCode.Space))
        {
          CancelInvoke(nameof(DecelerateCar));
          deceleratingCar = false;
          UseHandbrake();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
          RecoverTraction();
        }

        if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)))
        {
          ThrottleOff();
        }

        if ((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !deceleratingCar)
        {
          InvokeRepeating(nameof(DecelerateCar), 0f, 0.1f);
          deceleratingCar = true;
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && steeringAxis != 0f)
        {
          ResetSteeringAngle();
        }

        AnimateWheelMeshes();
    }
    
    public void MoveUp()
    {
      if (Mathf.Abs(localVelocityX) > 2.5f)
      {
        isDrifting = true;
        DriftCarPS();
      }
      else
      {
        isDrifting = false;
        DriftCarPS();
      }

      throttleAxis += (Time.deltaTime * 3f);
      if (throttleAxis > 1f)
      {
        throttleAxis = 1f;
      }

      if (localVelocityZ < -1f)
      {
        Brakes();
      }
      else
      {
        if (Mathf.RoundToInt(carSpeed) < _currentConfig.MaxForwardSpeed)
        {
          frontLeftCollider.brakeTorque = 0;
          frontLeftCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
          frontRightCollider.brakeTorque = 0;
          frontRightCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
          rearLeftCollider.brakeTorque = 0;
          rearLeftCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
          rearRightCollider.brakeTorque = 0;
          rearRightCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
        }
        else
        {
          frontLeftCollider.motorTorque = 0;
          frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
          rearRightCollider.motorTorque = 0;
        }
      }
    }
    public void MoveDown()
    {
      if (Mathf.Abs(localVelocityX) > 2.5f)
      {
        isDrifting = true;
        DriftCarPS();
      }
      else
      {
        isDrifting = false;
        DriftCarPS();
      }

      throttleAxis -= (Time.deltaTime * 3f);
      if (throttleAxis < -1f)
      {
        throttleAxis = -1f;
      }

      if (localVelocityZ > 1f)
      {
        Brakes();
      }
      else
      {
        if (Mathf.Abs(Mathf.RoundToInt(carSpeed)) < _currentConfig.MaxReverseSpeed)
        {
          frontLeftCollider.brakeTorque = 0;
          frontLeftCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
          frontRightCollider.brakeTorque = 0;
          frontRightCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
          rearLeftCollider.brakeTorque = 0;
          rearLeftCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
          rearRightCollider.brakeTorque = 0;
          rearRightCollider.motorTorque = (_currentConfig.AccelerationMultiplier * 50f) * throttleAxis;
        }
        else
        {

          frontLeftCollider.motorTorque = 0;
          frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
          rearRightCollider.motorTorque = 0;
        }
      }
    }   
    
    public void TurnRight()
    {
      steeringAxis += (Time.deltaTime * 10f * _currentConfig.SteeringSpeed);
      if (steeringAxis > 1f)
      {
        steeringAxis = 1f;
      }

      float steeringAngle = steeringAxis * _currentConfig.MaxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, _currentConfig.SteeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, _currentConfig.SteeringSpeed);
    }
    
    public void TurnLeft()
    {
      steeringAxis -= (Time.deltaTime * 10f * _currentConfig.SteeringSpeed);
      if (steeringAxis < -1f)
      {
        steeringAxis = -1f;
      }

      float steeringAngle = steeringAxis * _currentConfig.MaxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, _currentConfig.SteeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, _currentConfig.SteeringSpeed);
    }

    public void UseHandbrake()
    {
      CancelInvoke(nameof(RecoverTraction));

      driftingAxis += (Time.deltaTime);
      float secureStartingPoint = driftingAxis * FLWextremumSlip * _currentConfig.HandbrakeDriftMultiplier;

      if (secureStartingPoint < FLWextremumSlip)
      {
        driftingAxis = FLWextremumSlip / (FLWextremumSlip * _currentConfig.HandbrakeDriftMultiplier);
      }

      if (driftingAxis > 1f)
      {
        driftingAxis = 1f;
      }

      isDrifting = Mathf.Abs(localVelocityX) > 2.5f;

      if (driftingAxis < 1f)
      {
        FLwheelFriction.extremumSlip = FLWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        rearRightCollider.sidewaysFriction = RRwheelFriction;
      }

      isTractionLocked = true;
      DriftCarPS();
    }

    private void CarSounds()
    {
      if (carEngineSound != null)
      {
        float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.velocity.magnitude) / 25f);
        carEngineSound.pitch = engineSoundPitch;
      }

      if ((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f))
      {
        if (!tireScreechSound.isPlaying)
        {
          tireScreechSound.Play();
        }
      }
      else if ((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f))
      {
        tireScreechSound.Stop();
      }
    }

    private void ResetSteeringAngle()
    {
      if (steeringAxis < 0f)
      {
        steeringAxis += (Time.deltaTime * 10f * _currentConfig.SteeringSpeed);
      }
      else if (steeringAxis > 0f)
      {
        steeringAxis -= (Time.deltaTime * 10f * _currentConfig.SteeringSpeed);
      }

      if (Mathf.Abs(frontLeftCollider.steerAngle) < 1f)
      {
        steeringAxis = 0f;
      }

      var steeringAngle = steeringAxis * _currentConfig.MaxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, _currentConfig.SteeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, _currentConfig.SteeringSpeed);
    }

    private void AnimateWheelMeshes()
    {
      AnimateMesh(frontLeftCollider, frontLeftMesh.transform);
      AnimateMesh(frontRightCollider, frontRightMesh.transform);
      AnimateMesh(rearLeftCollider, rearLeftMesh.transform);
      AnimateMesh(rearRightCollider, rearRightMesh.transform);

      void AnimateMesh(WheelCollider wheelCollider, Transform meshTransform)
      {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        meshTransform.position = position;
        meshTransform.rotation = rotation;
      }
    }

    private void ThrottleOff()
    {
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;
    }

    private void DecelerateCar()
    {
      if (Mathf.Abs(localVelocityX) > 2.5f)
      {
        isDrifting = true;
        DriftCarPS();
      }
      else
      {
        isDrifting = false;
        DriftCarPS();
      }

      if (throttleAxis != 0f)
      {
        if (throttleAxis > 0f)
        {
          throttleAxis -= (Time.deltaTime * 10f);
        }
        else if (throttleAxis < 0f)
        {
          throttleAxis += (Time.deltaTime * 10f);
        }

        if (Mathf.Abs(throttleAxis) < 0.15f)
        {
          throttleAxis = 0f;
        }
      }

      carRigidbody.velocity *= (1f / (1f + (0.025f * _currentConfig.DecelerationMultiplier)));
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;

      if (carRigidbody.velocity.magnitude < 0.25f)
      {
        carRigidbody.velocity = Vector3.zero;
        CancelInvoke(nameof(DecelerateCar));
      }
    }

    private void Brakes()
    {
      frontLeftCollider.brakeTorque = _currentConfig.BrakeForce;
      frontRightCollider.brakeTorque = _currentConfig.BrakeForce;
      rearLeftCollider.brakeTorque = _currentConfig.BrakeForce;
      rearRightCollider.brakeTorque = _currentConfig.BrakeForce;
    }

    private void DriftCarPS()
    {
      if (isDrifting)
      {
        RLWParticleSystem.Play();
        RRWParticleSystem.Play();
      }
      else if (!isDrifting)
      {
        RLWParticleSystem.Stop();
        RRWParticleSystem.Stop();
      }

      if ((isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f)
      {
        RLWTireSkid.emitting = true;
        RRWTireSkid.emitting = true;
      }
      else
      {
        RLWTireSkid.emitting = false;
        RRWTireSkid.emitting = false;
      }
    }

    private void RecoverTraction()
    {
      isTractionLocked = false;
      driftingAxis -= (Time.deltaTime / 1.5f);
      if (driftingAxis < 0f)
      {
        driftingAxis = 0f;
      }

      if (FLwheelFriction.extremumSlip > FLWextremumSlip)
      {
        FLwheelFriction.extremumSlip = FLWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip * _currentConfig.HandbrakeDriftMultiplier * driftingAxis;
        rearRightCollider.sidewaysFriction = RRwheelFriction;

        Invoke(nameof(RecoverTraction), Time.deltaTime);

      }
      else if (FLwheelFriction.extremumSlip < FLWextremumSlip)
      {
        FLwheelFriction.extremumSlip = FLWextremumSlip;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip;
        rearRightCollider.sidewaysFriction = RRwheelFriction;

        driftingAxis = 0f;
      }
    }
  }
}
