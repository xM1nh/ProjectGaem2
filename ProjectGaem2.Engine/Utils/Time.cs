namespace ProjectGaem2.Engine.Utils
{
    public static class Time
    {
        public static float TotalTime;
        public static float DeltaTime;
        public static float FixedDeltaTime = 1000 / 30f;
        public static float TimeScale = 1.0f;

        public static void Update(float dt)
        {
            TotalTime += dt;
            DeltaTime += dt * TimeScale;
        }
    }
}
