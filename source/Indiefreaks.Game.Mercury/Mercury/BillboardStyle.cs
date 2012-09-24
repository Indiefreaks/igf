namespace ProjectMercury
{
    /// <summary>
    /// Billboard style tells the renderer if the emitter particles should be billboarded and in which way
    /// </summary>
    public enum BillboardStyle
    {
        /// <summary>
        /// Do not apply any billboarding
        /// </summary>
        None,
        /// <summary>
        /// Spherical billboards face the camera on all axis
        /// </summary>
        Spherical,
        /// <summary>
        /// Cylindrical billboards are allowed to rotate around a single axis in an attempt to face the camera
        /// the BillboardRotationalAxis property should also be set
        /// </summary>
        Cylindrical,
    }
}
