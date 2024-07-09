using Godot;

namespace killTheVegetables.scripts.Player;

public partial class Player : RigidBody3D
{
    [Export] private float movementSpeedGround = 15f;
    [Export] private float movementSpeedAir = 5f;

    [Export] private float dampingGround = 4f;
    [Export] private float dampingAir = 0f;
    
    [Export] private float jumpStrength = 30f;
	
    private Camera camera;
    private RayCast3D groundRaycast;
	
    public override void _Ready()
    {
        camera = GetNode<Camera>("Camera");
        groundRaycast = GetNode<RayCast3D>("GroundRaycast");
    }
    
    private bool isGrounded = false;
    private bool justJumped = false;

    public override void _PhysicsProcess(double delta)
    {
        if (!groundRaycast.IsColliding())
        {
            justJumped = false;
            isGrounded = false;
        }
        else if (!justJumped)
        {
            isGrounded = true;
        }

        float speed;
        if (isGrounded)
        {
            speed = movementSpeedGround;
            LinearDamp = dampingGround;
        }
        else
        {
            speed = movementSpeedAir;
            LinearDamp = dampingAir;
        }

        if (Input.IsActionPressed("jump") && isGrounded)
        {
            ApplyCentralImpulse(Vector3.Up * jumpStrength);
            justJumped = true;
            isGrounded = false;
        }
        
        var rawInputDirection = Vector3.Zero;

        if (Input.IsActionPressed("moveForwards"))
            rawInputDirection.Z -= 1f;
        if (Input.IsActionPressed("moveBackwards"))
            rawInputDirection.Z += 1f;
        if (Input.IsActionPressed("moveLeft"))
            rawInputDirection.X -= 1f;
        if (Input.IsActionPressed("moveRight"))
            rawInputDirection.X += 1f;

        var movementDirection = rawInputDirection.Rotated(Vector3.Up, Mathf.DegToRad(camera.yaw)).Normalized();
        ApplyCentralForce(movementDirection * speed * (float)delta * 100f);
    }
}