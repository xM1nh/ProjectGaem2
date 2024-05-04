namespace ProjectGaem2.Engine.Utils
{
    public static class Time
    {
        public static float TotalTime;
        public static float DeltaTime;

        public static float TimeScale = 1.0f;

        internal static float FixedDeltaTime = 1000 / 30f;
        internal static float Alpha = 0;

        public static float FixedAlpha => Alpha;

        public static void Update(float dt)
        {
            TotalTime += dt;
            DeltaTime += dt * TimeScale;
        }
    }
}
