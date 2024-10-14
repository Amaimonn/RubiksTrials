using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using R3;
using System;
using System.Text.RegularExpressions;

[UxmlElement]
public partial class ColorReplacer : VisualElement
{
    [UxmlAttribute]
    public List<Color> FillColors 
    { 
        get => _fillColors; 
        set 
        {
            _fillColors = value;
            SetBackgroundSVG();
            DefinePainterActions();
        }
    }

    [UxmlAttribute]
    public bool BoxSizing { get => _boxSizing ; set => _boxSizing  = value; }

    private List<Color> _fillColors = new();
    private bool _boxSizing = true;
    private string[] _hexColors = new string[27];
    private string _rubikSVG;
    private List<Action<Painter2D>> _painterActions = new();
    private float ScaleX => contentRect.width/272; 
    private float ScaleY => _boxSizing ? ScaleX : contentRect.height/265;

    public ColorReplacer()
    {
        generateVisualContent += GenerateRubik;
    }

    public static string ToRGBHex(Color c)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    public void SetBackgroundSVG()
    {
        for (int i = 0; i < 27; i++)
        {
            _hexColors[i] = i < _fillColors.Count ? ToRGBHex(_fillColors[i]) : ToRGBHex(Color.green);//ToRGBHex(FillColors[i]);
        }

        _rubikSVG = 
        @$"<svg width=""272"" height=""265"" viewBox=""0 0 272 265"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
            <path d=""M168.24 81L2 27.2667L126.58 1L270 23.6667L168.24 81Z"" fill=""#23201B"" stroke=""black""/>
            <path d=""M32.2059 173.437L2 27L168 80.5518L147.995 263L32.2059 173.437Z"" fill=""#24221D"" stroke=""black""/>
            <path d=""M270 24L168.881 80.8984L149 263L229.842 165.649L270 24Z"" fill=""#26241E"" stroke=""black""/>
            <path d=""M159 7.6875L127.518 2L101 8.36458L133.658 15L159 7.6875Z"" fill=""{_hexColors[0]}"" stroke=""black""/>
            <path d=""M205 14.3158L166 8L140 15.2368L180.459 23L205 14.3158Z"" fill=""{_hexColors[1]}"" stroke=""black""/>
            <path d=""M260 22.7847L211.779 15L188 23.8403L239.144 34L260 22.7847Z"" fill=""{_hexColors[2]}"" stroke=""black""/>
            <path d=""M129 15.8174L95.9336 9L63 16.6522L96.1992 25L129 15.8174Z"" fill=""{_hexColors[3]}"" stroke=""black""/>
            <path d=""M176 24.5906L135.154 16L102 25.5302L144.57 36L176 24.5906Z"" fill=""{_hexColors[4]}"" stroke=""black""/>
            <path d=""M235 35.6737L182.992 25L152 37.1789L207.466 51L235 35.6737Z"" fill=""{_hexColors[5]}"" stroke=""black""/>
            <path d=""M91 25.9595L57.1479 18L15 27.1149L47.9155 37L91 25.9595Z"" fill=""{_hexColors[6]}"" stroke=""black""/>
            <path d=""M139 37.0455L96.6983 26L54 38.6818L97.4914 53L139 37.0455Z"" fill=""{_hexColors[7]}"" stroke=""black""/>
            <path d=""M203 52.8615L146.773 39L105 55.0846L164.543 73L203 52.8615Z"" fill=""{_hexColors[8]}"" stroke=""black""/>
            <path d=""M41.1776 48.626L8 37L17.4408 86.939L49 102L41.1776 48.626Z"" fill=""{_hexColors[9]}"" stroke=""black""/>
            <path d=""M90.341 66.2L47 51L53.7803 105.267L93 125L90.341 66.2Z"" fill=""{_hexColors[10]}"" stroke=""black""/>
            <path d=""M156 89.3894L97 69L98.7236 128.315L150.564 154L156 89.3894Z"" fill=""{_hexColors[11]}"" stroke=""black""/>
            <path d=""M50.2355 108.529L19 93L26.9094 132.95L56 151L50.2355 108.529Z"" fill=""{_hexColors[12]}"" stroke=""black""/>
            <path d=""M93.1639 131.988L55 112L60.1148 155.329L95 178L93.1639 131.988Z"" fill=""{_hexColors[13]}"" stroke=""black""/>
            <path d=""M150 162.032L100 136L101.447 181.061L146.053 210L150 162.032Z"" fill=""{_hexColors[14]}"" stroke=""black""/>
            <path d=""M56.4656 157.29L29 139L34.9595 172.108L61 192L56.4656 157.29Z"" fill=""{_hexColors[15]}"" stroke=""black""/>
            <path d=""M95.292 182.277L61 160L65.2044 194.536L97 218L95.292 182.277Z"" fill=""{_hexColors[16]}"" stroke=""black""/>
            <path d=""M145 215.446L101 187L102.196 222.757L141.943 252L145 215.446Z"" fill=""{_hexColors[17]}"" stroke=""black""/>
            <path d=""M212 66L173.171 88.9436L166 153L200.771 125.017L212 66Z"" fill=""{_hexColors[18]}"" stroke=""black""/>
            <path d=""M243 47L216.028 63.5333L205 121L230.378 100.467L243 47Z"" fill=""{_hexColors[19]}"" stroke=""black""/>
            <path d=""M266 33L245.661 45.0251L233 97L252.665 81.3674L266 33Z"" fill=""{_hexColors[20]}"" stroke=""black""/>
            <path d=""M199 132L164.772 160.676L159 209L190.544 177.536L199 132Z"" fill=""{_hexColors[21]}"" stroke=""black""/>
            <path d=""M228 107L203.315 128.078L195 173L218.516 148.892L228 107Z"" fill=""{_hexColors[22]}"" stroke=""black""/>
            <path d=""M250 88L230.755 103.963L221 146L239.586 127.11L250 88Z"" fill=""{_hexColors[23]}"" stroke=""black""/>
            <path d=""M189 183L157.977 215.173L154 252L182.239 219.561L189 183Z"" fill=""{_hexColors[24]}"" stroke=""black""/>
            <path d=""M217 155L192.739 179.425L186 215L208.643 189.513L217 155Z"" fill=""{_hexColors[25]}"" stroke=""black""/>
            <path d=""M238 132L218.886 151.249L211 185L229.178 164.301L238 132Z"" fill=""{_hexColors[26]}"" stroke=""black""/>
        </svg>";
    }

    private void DefinePainterActions()
    {
        Regex lineSplitter = new (@"([ML]|=|Z| )");
        var splittedSVG = lineSplitter.Split(_rubikSVG
            .Replace("\"", "")
            .Replace("/>", " ")
            .Replace("=", " ")
            .Replace(".", ","))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToArray();

        //var fillIndex = 0;
        for (int i = 0; i < splittedSVG.Length; i++)
        {
            var current = i;
            //var fillIter = fillIndex;
            switch (splittedSVG[i])
            {
                case "M":
                    _painterActions.Add((Painter2D painter) => painter.BeginPath());
                    _painterActions.Add((Painter2D painter) => {
                        painter.MoveTo(new Vector2(float.Parse(splittedSVG[current+1])*ScaleX, 
                            float.Parse(splittedSVG[current+2])*ScaleY));
                    });
                    i += 2;
                    break;
                case "L":
                    _painterActions.Add((Painter2D painter) => painter.LineTo(new Vector2(float.Parse(splittedSVG[current+1])*ScaleX, 
                        float.Parse(splittedSVG[current+2])*ScaleY)));
                    i += 2;
                    break;
                case "fill":
                    // Debug.Log($"fill count: {FillingColors.Count}");
                    // if (fillIndex < _fillColors.Count)
                    // {
                    //     painterActions.Add((Painter2D painter) => painter.fillColor = _fillColors[fillIter]);
                    //     fillIndex++;
                    // }
                    // else
                    {
                        if (ColorUtility.TryParseHtmlString(splittedSVG[current+1], out Color fillColor))
                        {
                            _painterActions.Add((Painter2D painter) => painter.fillColor = fillColor);
                        }
                    }
                    _painterActions.Add((Painter2D painter) => painter.Fill());
                    i++;
                    break;
                case "stroke":
                    if (ColorUtility.TryParseHtmlString(splittedSVG[current+1], out Color strokeColor))
                    {
                        _painterActions.Add((Painter2D painter) => painter.strokeColor = strokeColor);
                    }
                    _painterActions.Add((Painter2D painter) => painter.Stroke());
                    _painterActions.Add((Painter2D painter) => painter.ClosePath());
                    break;
                default:
                    break;
            }
        }
    }

    private void GenerateRubik(MeshGenerationContext mgc)
    {
        var painter2D = mgc.painter2D;
        painter2D.lineWidth = 1.0f;
        foreach (var painterAction in _painterActions)
        {
            painterAction.Invoke(painter2D);
        }

        // Debug.Log("gen");
        // for(int i = 0; i < vertices0.Count; i++)
        // {
        //     var mesh = mgc.Allocate(vertices0[i].Length, indices0[i].Length);
        //     mesh.SetAllVertices(vertices0[i].ToArray());
        //     mesh.SetAllIndices(indices0[i].ToArray());
        // }

    }
}
