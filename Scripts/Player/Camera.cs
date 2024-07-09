using Godot;

namespace killTheVegetables.scripts.Player;

public partial class Camera : Camera3D
{
    public float yaw { get; private set; } = 0f;
    public float pitch { get; private set; } = 0f;

    [Export] private float sensitivity = 2f;
	
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;
        if (@event is not InputEventMouseMotion motion) return;
		
        yaw -= motion.Relative.X * sensitivity / 10f;
        if (yaw > 360f)
            yaw -= 360f;
        if (yaw < 0f)
            yaw += 360f;
				
        pitch -= motion.Relative.Y * sensitivity / 10f;
        if (pitch > 90f)
            pitch = 90f;
        if (pitch < -90f)
            pitch = -90f;
    }
	
    public override void _Process(double delta)
    {
        RotationDegrees = new Vector3(pitch, yaw, 0f);
    }
}