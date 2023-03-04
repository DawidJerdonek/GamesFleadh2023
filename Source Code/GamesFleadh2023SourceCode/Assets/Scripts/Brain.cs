using System;
using UnityEngine;

public class Brain
{
    private int numInputs;
    private int numHidden;
    private int numOutputs;
    private float[][] weightsLayer1;
    private float[] weightsLayer2;
    private float[] biases;
    private bool useBiases = true;

    public Brain()
    {
    }

    public void Init(int inputs, int hidden, int outputs)
    {
        numInputs = inputs;
        numHidden = hidden;
        numOutputs = outputs;
        Debug.Log("INIT AI " );


        weightsLayer1 = new float[inputs][];
        for (int i = 0; i < inputs; i++)
        {
            weightsLayer1[i] = new float[hidden];
        }

        for (int i = 0; i < inputs; i++)
        {
            for (int j = 0; j < hidden; j++)
            {
                float r = UnityEngine.Random.Range(-1f, 1f);
                weightsLayer1[i][j] = r;
            }
        }

        weightsLayer2 = new float[hidden];

        for (int i = 0; i < hidden; i++)
        {
            float r = UnityEngine.Random.Range(-1f, 1f);
            weightsLayer2[i] = r;
        }

        biases = new float[hidden + 1];

        for (int j = 0; j < hidden + 1; j++)
        {
            float r = UnityEngine.Random.Range(-1f, 1f);
            biases[j] = r;
        }
    }

    public int FeedForward(float[] inputs)
    {
        float output = 0.0f;
        float[] dot = new float[5];
        float[] soft = new float[5];
        float product = 0.0f;

        for (int i = 0; i < numHidden; i++)
        {
            product = 0.0f;
            for (int j = 0; j < numInputs; j++)
            {
                product = inputs[j] * weightsLayer1[j][i];
                dot[i] += product;
            }
            if (useBiases)
                dot[i] += biases[i];

            soft[i] = ReLu(dot[i]);
        }

        product = 0.0f;
        for (int i = 0; i < numHidden; i++)
        {
            product = soft[i] * weightsLayer2[i];
            output += product;
        }

        if (useBiases)
            output += biases[4];

        output = Sigmoid(output);

        if (output > 0.5f)
            return 1;
        else
            return 0;
    }

    private float ReLu(float x)
    {
        return Mathf.Max(0, x);
    }

    private float Sigmoid(float x)
    {
        return 1.0f / (1.0f + Mathf.Exp(-x));
    }
}