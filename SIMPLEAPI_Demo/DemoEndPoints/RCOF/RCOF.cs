﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoEndPoints.Clases;
namespace DemoEndPoints.RCOF
{
    public class RCOF
    {
        public Caratula Caratula { get; set; }
        public List<Resumen> Resumen { get; set; }
        public CertificadoDigital CertificadoDigital { get; set; }

        public RCOF()
        {
            this.Resumen = new List<Resumen>();
        }
    }
}