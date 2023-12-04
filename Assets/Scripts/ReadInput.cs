using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Compiler;
using System.IO;

using UnityEngine;


public class ReadInput : MonoBehaviour
{

    public float radius;
    public int segments;
    public float startAngle;
    public float endAngle;

    private string input;
    public GameObject point;

    private List<TokenM> tokens;
    // Start is called before the first frame update
    void Start()
    {
        radius = 100;
        segments = 30;
        startAngle = 0f;
        endAngle = 270f;

        // Create the circle
        GameObject circle = new GameObject("Circunference");
        circle.transform.position = new Vector3(360f, 400f, 0f);
        circle.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = circle.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateCircleSprite(radius, segments, startAngle, endAngle);

        //// Draw a line
        //Vector2 startPoint = new Vector2(100f, 200f);
        //Vector2 endPoint = new Vector2(500f, 400f);
        //DrawLine(startPoint, endPoint, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadStringInput(string s)
    {
        List<string> importedFiles = new List<string>();
        input = s;
        Debug.Log(s);
        tokens = LexerM.Lex(input);

        ParserM parser = new ParserM(tokens);
        bool isImport = false;
        do
        {
            isImport = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.WhiteSpaceToken || tokens[i].Type==TokenType.EOF)
                {
                    tokens.RemoveAt(i);
                }
            }
            for (int i = 0; i < tokens.Count; i++)
            {
                Debug.Log("iterando tokens");
                if (tokens[i].Type == TokenType.ImportToken)
                {
                    Debug.Log("ecnontrado import token "+i+" sig token "+tokens[i+1].Value + " "+tokens[i+1].Type);
                    isImport = true;
                    if (tokens[i + 1].Type == TokenType.StringToken)
                    {
                        Debug.Log("ecnontrado string token "+i);
                        string documentName = tokens[i + 1].Value; // Nombre del documento a leer
                        Debug.Log(documentName);
                        
                        if (!importedFiles.Contains(documentName))
                        {
                            importedFiles.Add(documentName);
                            string text = File.ReadAllText(documentName);
                            Debug.Log("text ====> : " + text);
                            List<TokenM> importedTokens = LexerM.Lex(text); // Leer y analizar el contenido del documento
                            tokens.InsertRange(i + 2, importedTokens); // Insertar los tokens del documento importado en la lista de tokens del nuevo documento
                        }

                        tokens.RemoveAt(i+1);
                        tokens.RemoveAt(i);
                    }
                }
            }
            
            
        } while(isImport);

        
        
        
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Type == TokenType.WhiteSpaceToken || tokens[i].Type==TokenType.EOF)
            {
                tokens.RemoveAt(i);
            }
        }


        tokens.Add(new TokenM(TokenType.EOF, ""));


        foreach (var token in tokens)
        {
            Debug.Log(token.Type.ToString());
        }

        MainProgramNode main = parser.Parse();


        
        foreach (var item in main.Body)
        {
            Debug.Log(item);
        }

        Evaluator evaluator = new Evaluator();

        string output = evaluator.EvaluateMain(main);

        List<Instaciable> pointDeclarationNodes = evaluator.result;
        Debug.Log(pointDeclarationNodes.Count);

        foreach (var item in pointDeclarationNodes)
        {
            switch (item)
            {
                case Point a:
                    Vector3 whereGenerate = new Vector3(a.X, a.Y, 0);
                    Instantiate(point, whereGenerate, transform.rotation);
                    break;
                case Line line:

                    Vector2 startPoint = new Vector2(line.X.X, line.X.Y);
                    Vector2 endPoint = new Vector2(line.Y.X, line.Y.Y);
                    DrawLine(startPoint, endPoint, 2);
                    break;
                case Circle c:
                    radius = (float)c.Radius;
                    segments = 30;
                    startAngle = 0f;
                    endAngle = 360f;

                    // Create the circle
                    GameObject circle = new GameObject("Circunference");
                    circle.transform.position = new Vector3(c.Center.X, c.Center.Y, 0f);
                    circle.AddComponent<SpriteRenderer>();
                    SpriteRenderer spriteRenderer = circle.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = CreateCircleSprite(radius, segments, startAngle, endAngle);

                    break;
                default:
                    break;
            }
        }
        
        
        
        
        Debug.Log("hello world");

    }

    Sprite CreateCircleSprite(float radius, int segments, float startAngle, float endAngle)
    {
        Texture2D texture = new Texture2D((int)(2 * radius) + 1, (int)(2 * radius) + 1);
        Color[] pixels = new Color[texture.width * texture.height];

        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % texture.width;
            int y = i / texture.width;
            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(radius, radius));
            float angle = Mathf.Atan2(y - radius, x - radius) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360f;
            }

            if (distance <= radius && distance >= radius - 3 && angle >= startAngle && angle <= endAngle)
            {
                pixels[i] = Color.white;
            }
            else
            {
                pixels[i] = Color.clear;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 1f);
    }

    void DrawLine(Vector2 startPoint, Vector2 endPoint, int thickness)
    {
        GameObject line = new GameObject("Line");
        line.transform.position = startPoint;
        line.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = line.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateLineSprite(startPoint, endPoint, thickness);
        line.transform.localScale = new Vector3(Vector2.Distance(startPoint, endPoint), thickness, 1f);
        line.transform.rotation = Quaternion.Euler(0f, 0f, GetAngle(startPoint, endPoint));
    }

    Sprite CreateLineSprite(Vector2 startPoint, Vector2 endPoint, int thickness)
    {
        Texture2D texture = new Texture2D((int)(Vector2.Distance(startPoint, endPoint)), thickness);
        Color[] pixels = new Color[texture.width * texture.height];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        Rect rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 1f);
    }

    float GetAngle(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 direction = endPoint - startPoint;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
