using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class GUISuperMarioPlatform2DMask : ShaderGUI
{
    private enum Expandable
    {
        SurfaceInputs = 1 << 0,
    }

    private class Labels
    {
        public static readonly string[] InputsLabel = Enum.GetNames(typeof(Expandable));

        public static readonly GUIContent SurfaceInputsLabel = EditorGUIUtility.TrTextContent("Surface Inputs", "Surface Inputs");
        public static readonly GUIContent MaskRadiusLabel = EditorGUIUtility.TrTextContent("Mask Radius", "The Radius of the mask!");
        public static readonly GUIContent MaskSmoothnessLabel = EditorGUIUtility.TrTextContent("Mask Smoothness", "The smoothness of the mask!");
        public static readonly GUIContent MaskMainTextureLabel = EditorGUIUtility.TrTextContent("Texture", "The Main texture!");
    }

    private MaterialEditor MaterialEditor { get; set; }
    private MaterialProperty MaskRadiusProperty { get; set; }
    private MaterialProperty MaskSmoothnessProperty { get; set; }
    private MaterialProperty MaskMainTextureProperty { get; set; }
    private MaterialProperty MaskColorProperty { get; set; }

    private readonly string _maskRadiusShaderProperty = "_MaskRadius";
    private readonly string _maskSmoothnessShaderProperty = "_MaskSmoothness";
    private readonly string _maskMainTextureProperty = "_MainTex";
    private readonly string _maskColorProperty = "_MaskColor";
    
    private bool _firstTimeApply = true;
    readonly MaterialHeaderScopeList _materialScopeList = new MaterialHeaderScopeList(uint.MaxValue & ~(uint)Expandable.SurfaceInputs);

    private void FindProperties(MaterialProperty[] properties)
    {
        Material material = MaterialEditor?.target as Material;
        if (material == null)
        {
            return;
        }

        MaskRadiusProperty = FindProperty(_maskRadiusShaderProperty, properties, true);
        MaskSmoothnessProperty = FindProperty(_maskSmoothnessShaderProperty, properties, true);
        MaskMainTextureProperty = FindProperty(_maskMainTextureProperty, properties, true);
        MaskColorProperty = FindProperty(_maskColorProperty, properties, true);

    }

    public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] properties)
    {
        if (materialEditorIn == null)
        {
            throw new ArgumentNullException("materialEditorIn");
        }

        MaterialEditor = materialEditorIn;
        Material material = MaterialEditor.target as Material;
        
        FindProperties(properties);

        if (_firstTimeApply)
        {
            OnOpenGUI(material, materialEditorIn);
            _firstTimeApply = false;
        }

        ShaderPropertiesGUI(material);
    }
    
    protected virtual uint _materialFilter => uint.MaxValue;

    private void OnOpenGUI(Material material, MaterialEditor materialEditor)
    {
        var filter = (Expandable)_materialFilter;

        if (filter.HasFlag(Expandable.SurfaceInputs))
        {
            _materialScopeList.RegisterHeaderScope(Labels.SurfaceInputsLabel, (uint)Expandable.SurfaceInputs, DrawSurfaceInputs);
        }
    }

    private void DrawSurfaceInputs(Material obj)
    {
        MaterialEditor.TexturePropertySingleLine(Labels.MaskMainTextureLabel, MaskMainTextureProperty, MaskColorProperty);
        MaterialEditor.IntSliderShaderProperty(MaskRadiusProperty, Labels.MaskRadiusLabel);
        MaterialEditor.ShaderProperty(MaskSmoothnessProperty, Labels.MaskSmoothnessLabel.ToString());
    }

    private void ShaderPropertiesGUI(Material material)
    {
        _materialScopeList.DrawHeaders(MaterialEditor, material);
    }
}
