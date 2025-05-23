/*
 * Copyright (c) [2023] [AD STUDIOS & VOLVE STUDIOS]
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are NOT permitted. You may only use this package to make
 * games.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */
namespace S3DShapes
{
    using System.Collections.Generic;
    using UnityEngine;

    public class S3DEqToVertices : MonoBehaviour
    {
        public Vector2 inputRange = new Vector2(-1f, 1f);
        public int points = 10;
        public string eq = "sqrt(1-(x)^2)";
        public bool polar = true;
        public string eqx = "cos(t)";
        public string eqy = "sin(t)";

        // Start is called before the first frame update
        public List<Vector2> GetVerticesFromEquation()
        {
            List<Vector2> vertices = new List<Vector2>();
            if (!polar)
            {
                for (float i = inputRange.x; i <= inputRange.y; i += (inputRange.y - inputRange.x) / (points - 1))
                {
                    ExpressionEvaluator.Evaluate(eq.Replace("x", i + ""), out float val);
                    vertices.Add(new Vector2(i, val));
                }
            }
            else
            {
                for (float i = inputRange.x; i <= inputRange.y; i += (inputRange.y - inputRange.x) / (points - 1))
                {
                    ExpressionEvaluator.Evaluate(eqx.Replace("t", i + ""), out float valX);
                    ExpressionEvaluator.Evaluate(eqy.Replace("t", i + ""), out float valY);
                    vertices.Add(new Vector2(valX, valY));
                }
            }
            return vertices;
        }

        public static S3DFace GenerateFaceFromEquation(string equationX, string equationY, int vertex)
        {
            List<Vector2> vertices = new List<Vector2>();
            float segment = (2f * Mathf.PI) / (vertex);
            for (float i = 0; i < vertex; i++)
            {
                ExpressionEvaluator.Evaluate(equationX.Replace("t", i * segment + ""), out float valX);
                ExpressionEvaluator.Evaluate(equationY.Replace("t", i * segment + ""), out float valY);
                vertices.Add(new Vector2(valX, valY));
            }

            return new S3DFace(vertices);
        }

        public static S3DFace GenerateFaceFromEquation(string equationX, string equationY, int vertex, int endVertex)
        {
            List<Vector2> vertices = new List<Vector2>();
            float segment = (2f * Mathf.PI) / (vertex);
            for (float i = 0; i < endVertex; i++)
            {
                ExpressionEvaluator.Evaluate(equationX.Replace("t", i * segment + ""), out float valX);
                ExpressionEvaluator.Evaluate(equationY.Replace("t", i * segment + ""), out float valY);
                vertices.Add(new Vector2(valX, valY));
            }

            return new S3DFace(vertices);
        }

    }
}