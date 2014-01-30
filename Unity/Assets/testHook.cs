﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Rendering;
using Engine;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using Engine.Simulation;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets
{
    public class TestHook : MonoBehaviour
    {
        private VectorFieldRenderer fieldRenderer;
        private IPolyhedron polyhedron;
        
        private VelocityFieldFactory velocityFieldFactory;
        private FieldUpdater updater;

        private PrognosticFields<Face> fields;
        private PrognosticFields<Face> oldFields;
        private PrognosticFields<Face> olderFields;

        private FieldOperators _operators;
        private FieldIntegrator _integrator;

        // Use this for initialization
        void Start ()
        {
            var options = new Options {MinimumNumberOfFaces = 101, Radius = 6000};
            polyhedron = GeodesicSphereFactory.Build(options);
            new PolyhedronRenderer(polyhedron, "Surface", "Materials/Wireframe", "Materials/Surface");

            velocityFieldFactory = new VelocityFieldFactory(polyhedron);
            fieldRenderer = new VectorFieldRenderer(polyhedron, "vectors", "Materials/Vectors");

            var fieldsFactory = new PrognosticFieldsFactory(polyhedron);
            fieldsFactory.Height = fieldsFactory.XDependentField(8, 1);
            fields = fieldsFactory.Build();

            var parameters = new SimulationParameters
            {
                RotationFrequency = 1.0/(24.0*3600.0)*0.4,
                Gravity = 10.0/1000.0,
                NumberOfRelaxationIterations = 300,
                Timestep = 300
            };

            updater = new FieldUpdater(polyhedron, parameters);

            var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);

            fieldRenderer.Update(velocityField);

            _operators = new FieldOperators(polyhedron);
            _integrator = new FieldIntegrator(polyhedron, parameters);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKey(KeyCode.N))
            {
                for (int i = 0; i < 1; i++)
                {
                    //Debug.Log(fields.ToString(0));
                    olderFields = oldFields;
                    oldFields = fields;
                    fields = updater.Update(fields, oldFields, olderFields);
                    //Debug.Log(fields.Streamfunction);
                    //Debug.Log(_operators.Laplacian(fields.Streamfunction));
                    //var coriolis = SimulationUtilities.CoriolisField(polyhedron, 1.0 / (24.0 * 3600.0));
                    //var f = fields.AbsoluteVorticity - coriolis;
                    //var sf = _integrator.Integrate(oldFields.Streamfunction, f);
                    //var error = (_operators.Laplacian(sf) - f).Select(x => Math.Abs(x));
                    //Debug.Log("Max f: " + f.Max());
                    //Debug.Log("Max error: " + error.Max());
                    //Debug.Log("Max SF:" + fields.Streamfunction.Max());

                    var velocityField = velocityFieldFactory.VelocityField(fields.Streamfunction, fields.VelocityPotential);
                    //Debug.Log(velocityField.Values.Max(value => 1000*value.Norm()));

                    fieldRenderer.Update(velocityField);
                }
            }
        }
    }
}
