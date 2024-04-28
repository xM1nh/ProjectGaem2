namespace ProjectGaem2.Engine.Utils
{
    public static class Time
    {
        public static float TotalTime { get; set; }
        public static float DeltaTime { get; set; }
        public static float TimeScale { get; set; } = 1.0f;

        public static void Update(float dt)
        {
            TotalTime += dt;
            DeltaTime += dt * TimeScale;
        }
    }
}
