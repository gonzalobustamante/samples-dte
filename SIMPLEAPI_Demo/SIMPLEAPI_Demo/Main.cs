﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIMPLEAPI_Demo
{
    public partial class Main : Form
    {
        Handler handler = new Handler();

        public Main()
        {
            InitializeComponent();
        }

        #region Emision de Documentos

        private void botonIngresarTimbraje_Click(object sender, EventArgs e)
        {
            IngresarTimbraje formulario = new IngresarTimbraje();
            formulario.ShowDialog();
        }

        private void botonGenerarDocumento_Click(object sender, EventArgs e)
        {


            //El folio es obligatorio
            handler.Folio = 168;
            //El Id debe ser alfanumerico. Remitirse a letras y números
            handler.idDte = "TESTPRUEBA2";
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronica;
            var dte = handler.GenerateDTE();
            handler.GenerateDetails(dte);

            handler.usaReferencia = false;
            handler.Referencias(dte);

            var path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");

            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            MessageBox.Show("Documento generado exitosamente en " + path);
        }

        private void botonGenerarEnvio_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] pathFiles = openFileDialog1.FileNames;
            List<ChileSystems.DTE.Engine.Documento.DTE> dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            List<string> xmlDtes = new List<string>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);

                /*Generar envio para el SII
                Un envío puede contener 1 o varios DTE. No es necesario que sean del mismo tipo,
                es decir, en un envío pueden ir facturas electrónicas afectas, notas de crédito, guias de despacho,
                etc.             
                 */
                dtes.Add(dte);
                xmlDtes.Add(xml);
            }
            var EnvioSII = handler.GenerarEnvioDTEToSII(dtes, xmlDtes);

            /*Generar envio para el cliente
            En esencia es lo mismo que para el SII */
            //var EnvioCliente = GenerarEnvioCliente(dte, xml);

            /*Puede ser el EnvioSII o EnvioCliente, pues es el mismo tipo de objeto*/
            var filePath = EnvioSII.Firmar(handler.nombreCertificado, handler.serialKEY, true);
            handler.Validate(filePath, SIMPLE_SDK.Security.Firma.Firma.TipoXML.Envio, ChileSystems.DTE.Engine.XML.Schemas.EnvioDTE);
            MessageBox.Show("Envío generado exitosamente en " + filePath);
        }

        private void botonEnviarSii_Click(object sender, EventArgs e)
        {
            /*Procedemos a enviar el 'Envío' al SII, que no es otra cosa que simular un upload vía browser*/
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;
            long trackId = handler.EnviarEnvioDTEToSII(pathFile, handler.serialKEY, radioProduccion.Checked);
            MessageBox.Show("Sobre enviado correctamente. TrackID: " + trackId.ToString());
        }

        #endregion

        #region Simulacion

        private void botonSimulacion_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            List<ChileSystems.DTE.Engine.Documento.DTE> dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            List<string> xmlDtes = new List<string>();
            /*Cada valor de i se asigna como folio. Debes tener ojo con no enviar documentos con folios ya utilizados y enviados.*/
            for (int i = 31; i <= 50; i++)
            {
                var dteAux = handler.GenerateRandomDTE(i, ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronica);
                string filePath = handler.TimbrarYFirmarXMLDTE(dteAux, "out\\temp\\", "out\\caf\\");
                string xml = File.ReadAllText(filePath, Encoding.GetEncoding("ISO-8859-1"));
                dtes.Add(dteAux);
                xmlDtes.Add(xml);
            }

            var dteAux2 = handler.GenerateRandomDTE(33, ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica);
            var filePath2 = handler.TimbrarYFirmarXMLDTE(dteAux2, "out\\temp\\", "out\\caf\\");
            var xml2 = File.ReadAllText(filePath2, Encoding.GetEncoding("ISO-8859-1"));
            dtes.Add(dteAux2);
            xmlDtes.Add(xml2);

            var dteAux3 = handler.GenerateRandomDTE(23, ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaDebitoElectronica);
            var filePath3 = handler.TimbrarYFirmarXMLDTE(dteAux3, "out\\temp\\", "out\\caf\\");
            var xml3 = File.ReadAllText(filePath3, Encoding.GetEncoding("ISO-8859-1"));
            dtes.Add(dteAux3);
            xmlDtes.Add(xml3);

            var dteAux4 = handler.GenerateRandomDTE(19, ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.FacturaCompraElectronica);
            var filePath4 = handler.TimbrarYFirmarXMLDTE(dteAux4, "out\\temp\\", "out\\caf\\");
            var xml4 = File.ReadAllText(filePath4, Encoding.GetEncoding("ISO-8859-1"));
            dtes.Add(dteAux4);
            xmlDtes.Add(xml4);

            var EnvioSII = handler.GenerarEnvioDTEToSII(dtes, xmlDtes);
            string filePathEnvio = EnvioSII.Firmar(handler.nombreCertificado, handler.serialKEY, true);
            MessageBox.Show("Envío generado exitosamente en " + filePathEnvio);
        }

        

        private void botonEnviarSimulacionSII_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;
            handler.EnviarEnvioDTEToSII(pathFile, "SERIALKEY", radioProduccion.Checked);
        }

        #endregion

        #region Boletas Electronicas

        private void botonGenerarBoleta_Click(object sender, EventArgs e)
        {
            GenerarBoletaElectronica formulario = new GenerarBoletaElectronica();
            formulario.ShowDialog();
        }

        private void botonGenerarRCOF_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] pathFiles = openFileDialog1.FileNames;
            List<ChileSystems.DTE.Engine.Documento.DTE> dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dte);
            }
            var rcof = handler.GenerarRCOF(dtes);
            rcof.DocumentoConsumoFolios.Id = "RCOF_" + DateTime.Now.Ticks.ToString();
            /*Firmar retorna además a través de un out, el XML formado*/
            string xmlString = string.Empty;
            var filePathArchivo = rcof.Firmar(handler.nombreCertificado, handler.serialKEY, out xmlString);
            MessageBox.Show("RCOF Generado correctamente en " + filePathArchivo);
        }

        private void botonLibroBoletas_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] pathFiles = openFileDialog1.FileNames;
            List<ChileSystems.DTE.Engine.Documento.DTE> dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dte);
            }
            var libro = handler.GenerateLibroBoletas(dtes);
            libro.EnvioLibro.Caratula.FolioNotificacion = 1;
            libro.EnvioLibro.Id = "L_BOLETAS_" + DateTime.Now.Ticks.ToString();
            var filePathArchivo = libro.Firmar(handler.nombreCertificado, handler.serialKEY);
            MessageBox.Show("Libro boletas Generado correctamente en " + filePathArchivo);
        }

        private void botonAnularDocumento_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;
            string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
            var dteBoleta = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);

            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica;
            handler.Folio = 9;
            handler.idDte = "NC_BOLELEC_3";

            var dteNC = handler.GenerateDTE();
            /*En el caso de las anulaciones, los detalles y totales son los mismos que el documento de origen*/
            dteNC.Documento.Detalles = dteBoleta.Documento.Detalles;
            dteNC.Documento.Encabezado.Totales = dteBoleta.Documento.Encabezado.Totales;
            dteNC.Documento.Referencias = new List<ChileSystems.DTE.Engine.Documento.Referencia>();
            dteNC.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.AnulaDocumentoReferencia,
                FechaDocumentoReferencia = DateTime.Now,
                //Folio de Referencia = Debe ir el folio del documento que estás referenciando
                FolioReferencia = dteBoleta.Documento.Encabezado.IdentificacionDTE.Folio.ToString(),
                IndicadorGlobal = 0,
                Numero = 1,
                RazonReferencia = "ANULA BOLETA ELECTRÓNICA",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.BoletaElectronica
            });

            var path = handler.TimbrarYFirmarXMLDTE(dteNC, "out\\temp\\", "out\\caf\\");
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);

            MessageBox.Show("Nota de crédito generada exitosamente en " + path);
        }

        private void botonRebajaDocumento_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;
            string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
            var dteBoleta = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);

            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica;
            handler.Folio = 11;
            handler.idDte = "NC_BOLELEC_6";
            var dteNC = handler.GenerateDTE();
            /*En el caso de las anulaciones, los detalles y totales son los mismos que el documento de origen*/
            dteNC.Documento.Detalles = dteBoleta.Documento.Detalles;
            dteNC.Documento.Encabezado.Totales = dteBoleta.Documento.Encabezado.Totales;
            dteNC.Documento.Referencias = new List<ChileSystems.DTE.Engine.Documento.Referencia>();
            dteNC.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.CorrigeMontos,
                FechaDocumentoReferencia = DateTime.Now,
                //Folio de Referencia = Debe ir el folio del documento que estás refenciando
                FolioReferencia = dteBoleta.Documento.Encabezado.IdentificacionDTE.Folio.ToString(),
                IndicadorGlobal = 0,
                Numero = 1,
                RazonReferencia = "CORRIGE BOLETA ELECTRÓNICA",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.BoletaElectronica
            });

            /*Calculo para el caso de una rebaja de un 40%*/
            double porc_descuento = 0.4;
            var neto = dteNC.Documento.Encabezado.Totales.MontoNeto - (dteNC.Documento.Encabezado.Totales.MontoNeto * porc_descuento);
            int netoInt = (int)Math.Round(neto, 0);
            int iva = (int)Math.Round(neto * 0.19, 0);
            int total = netoInt + iva;
            dteNC.Documento.Encabezado.Totales.MontoNeto = netoInt;
            dteNC.Documento.Encabezado.Totales.IVA = iva;
            dteNC.Documento.Encabezado.Totales.MontoTotal = total;

            dteNC.Documento.DescuentosRecargos = new List<ChileSystems.DTE.Engine.Documento.DescuentosRecargos>();
            dteNC.Documento.DescuentosRecargos.Add(new ChileSystems.DTE.Engine.Documento.DescuentosRecargos()
            {
                Descripcion = "DESCUENTO COMERCIAL",
                Numero = 1,
                TipoMovimiento = ChileSystems.DTE.Engine.Enum.TipoMovimiento.TipoMovimientoEnum.Descuento,
                TipoValor = ChileSystems.DTE.Engine.Enum.ExpresionDinero.ExpresionDineroEnum.Porcentaje,
                Valor = porc_descuento * 100,
            });

            var path = handler.TimbrarYFirmarXMLDTE(dteNC, "out\\temp\\", "out\\caf\\");
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);

            MessageBox.Show("Nota de crédito generada exitosamente en " + path);
        }

        private void botonGenerarEnvioBoleta_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            string[] pathFiles = openFileDialog1.FileNames;
            List<ChileSystems.DTE.Engine.Documento.DTE> dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            List<string> xmlDtes = new List<string>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dte = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dte);
                xmlDtes.Add(xml);
            }
            var EnvioSII = handler.GenerarEnvioBoletaDTEToSII(dtes, xmlDtes);
            var filePath = EnvioSII.Firmar(handler.nombreCertificado, handler.serialKEY, true);
            try
            {
                handler.Validate(filePath, SIMPLE_SDK.Security.Firma.Firma.TipoXML.EnvioBoleta, ChileSystems.DTE.Engine.XML.Schemas.EnvioBoleta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Envío generado exitosamente en " + filePath);
        }
        #endregion

        #region Utilitarios

        private void botonAceptacion_Click(object sender, EventArgs e)
        {
            /*
                 * ACD: Acepta Contenido del Documento
                 * RCD: Reclamo al Contenido del Documento
                 * ERM: Otorga Recibo de Mercaderías o Servicios
                 * RFP: Reclamo por Falta Parcial de Mercaderías
                 * RFT: Reclamo por Falta Total de Mercaderías
                 */
            int tipoDocumento = 33;
            int folio = 17158136;
            string accion = "ACD";
            string rutProveedor = "88888888";
            int dvProveedor = 8;
            var respuesta = handler.EnviarAceptacionReclamo(tipoDocumento, folio, accion, rutProveedor, dvProveedor, true);
            MessageBox.Show(respuesta);
        }

        private void botonMuestraImpresa_Click(object sender, EventArgs e)
        {
            MuestraTimbre formulario = new MuestraTimbre();
            formulario.ShowDialog();
        }
        private void botonConsultarEstadoDTE_Click(object sender, EventArgs e)
        {
            var responseEstadoDTE = handler.ConsultarEstadoDTE(radioProduccion.Checked);
            if (responseEstadoDTE != null)
            {
                MessageBox.Show(responseEstadoDTE.ResponseXml);
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error");
            }
        }
        #endregion

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void botonValidador_Click(object sender, EventArgs e)
        {
            Validador formulario = new Validador();
            formulario.ShowDialog();
        }

        private void botonSetPruebas_Click(object sender, EventArgs e)
        {
            handler.usaReferencia = false;

            List<string> pathFiles = new List<string>();
            List<int> folios = new List<int>();

            string nAtencion = "1092644";

            for (int i = 27; i<= 30; i++) //4 facturas
                folios.Add(i);
            for (int i = 14; i <= 16; i++) // 3 notas de credito
                folios.Add(i);            
            folios.Add(6); //1 nota de debito

           

            #region DTEs
            /******************************/
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.FacturaElectronica;
            handler.Folio = folios[0]; 
            handler.idDte = "A_" + folios[0];
            handler.casoPruebas = "CASO " + nAtencion + "-1";
            var dte = handler.GenerateDTE();

            var detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 139,
                Nombre = "Cajón AFECTO",
                Precio = 1838,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 59,
                Nombre = "Relleno AFECTO",
                Precio = 3021,
                Afecto = true
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            var path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.Folio = folios[1];
            handler.idDte = "A_" + folios[1];
            handler.casoPruebas = "CASO " + nAtencion + "-2";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 422,
                Nombre = "Pañuelo AFECTO",
                Precio = 3334,
                Descuento = 6,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 354,
                Nombre = "ITEM 2 AFECTO",
                Precio = 2393,
                Descuento = 12,
                Afecto = true
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.Folio = folios[2];
            handler.idDte = "A_" + folios[2];
            handler.casoPruebas = "CASO " + nAtencion + "-3";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 32,
                Nombre = "Pintura B&W AFECTO",
                Precio = 3820,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 180,
                Nombre = "ITEM 2 AFECTO",
                Precio = 3261,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 1,
                Nombre = "ITEM 3 SERVICIO EXENTO",
                Precio = 34914,
                Afecto = false
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.Folio = folios[3];
            handler.idDte = "A_" + folios[3];
            handler.casoPruebas = "CASO " + nAtencion + "-4";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 200,
                Nombre = "ITEM 1 AFECTO",
                Precio = 3186,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 85,
                Nombre = "ITEM 2 AFECTO",
                Precio = 3473,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 2,
                Nombre = "ITEM 3 SERVICIO EXENTO",
                Precio = 6791,
                Afecto = false
            });

            dte.Documento.DescuentosRecargos = new List<ChileSystems.DTE.Engine.Documento.DescuentosRecargos>();
            dte.Documento.DescuentosRecargos.Add(new ChileSystems.DTE.Engine.Documento.DescuentosRecargos()
            {
                Descripcion = "DESCUENTO COMERCIAL",
                Numero = 1,
                TipoMovimiento = ChileSystems.DTE.Engine.Enum.TipoMovimiento.TipoMovimientoEnum.Descuento,
                TipoValor = ChileSystems.DTE.Engine.Enum.ExpresionDinero.ExpresionDineroEnum.Porcentaje,
                Valor = 12
            });

            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica;
            handler.Folio = folios[4];
            handler.idDte = "C_" + folios[4];
            handler.casoPruebas = "CASO " + nAtencion + "-5";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Nombre = "CORRIGE GIRO DEL RECEPTOR",
                Afecto = true
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            dte.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.CorrigeTextoDocumentoReferencia,
                FechaDocumentoReferencia = DateTime.Now,
                FolioReferencia = folios[0].ToString(),
                IndicadorGlobal = 0,
                Numero = 2,
                RazonReferencia = "CORRIGE GIRO DEL RECEPTOR",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.FacturaElectronica
            });
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica;
            handler.Folio = folios[5];
            handler.idDte = "C_" + folios[5];
            handler.casoPruebas = "CASO " + nAtencion + "-6";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 155,
                Nombre = "Pañuelo AFECTO",
                Precio = 3334,
                Descuento = 6,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 240,
                Nombre = "ITEM 2 AFECTO",
                Precio = 2393,
                Descuento = 12,
                Afecto = true
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            dte.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.CorrigeMontos,
                FechaDocumentoReferencia = DateTime.Now,
                FolioReferencia = folios[1].ToString(),
                IndicadorGlobal = 0,
                Numero = 2,
                RazonReferencia = "DEVOLUCIÓN DE MERCADERÍAS",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.FacturaElectronica
            });
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica;
            handler.Folio = folios[6];
            handler.idDte = "C_" + folios[6];
            handler.casoPruebas = "CASO " + nAtencion + "-7";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 32,
                Nombre = "Pintura B&W AFECTO",
                Precio = 3820,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 180,
                Nombre = "ITEM 2 AFECTO",
                Precio = 3261,
                Afecto = true
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 1,
                Nombre = "ITEM 3 SERVICIO EXENTO",
                Precio = 34914,
                Afecto = false
            });

            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            dte.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.AnulaDocumentoReferencia,
                FechaDocumentoReferencia = DateTime.Now,
                FolioReferencia = folios[2].ToString(),
                IndicadorGlobal = 0,
                Numero = 2,
                RazonReferencia = "ANULA FACTURA",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.FacturaElectronica
            });
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/


            /********************************/
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaDebitoElectronica;
            handler.Folio = folios[7];
            handler.idDte = "D_" + folios[7];
            handler.casoPruebas = "CASO " + nAtencion + "-8";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Nombre = "CORRIGE GIRO DEL RECEPTOR",
                Afecto = true
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            dte.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.AnulaDocumentoReferencia,
                FechaDocumentoReferencia = DateTime.Now,
                FolioReferencia = folios[4].ToString(),
                IndicadorGlobal = 0,
                Numero = 2,
                RazonReferencia = "ANULA NOTA DE CREDITO",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.NotaCreditoElectronica
            });
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/
            #endregion

            #region Envio de Documentos

            List<ChileSystems.DTE.Engine.Documento.DTE> dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            List<string> xmlDtes = new List<string>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dteAux = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dteAux);
                xmlDtes.Add(xml);
            }
            var EnvioSII = handler.GenerarEnvioDTEToSII(dtes, xmlDtes);
            path = EnvioSII.Firmar(handler.nombreCertificado, handler.serialKEY, true);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.Envio, ChileSystems.DTE.Engine.XML.Schemas.EnvioDTE);
            MessageBox.Show("Envío generado exitosamente en " + path);

            #endregion

            #region Libro de VENTAS

            var libroVentas = handler.GenerateLibroVentas(EnvioSII);
            path = libroVentas.Firmar(handler.nombreCertificado, "out\\temp\\", handler.serialKEY);

            MessageBox.Show("Libro ventas guardado en " + path);
            #endregion

            #region Libro de COMPRAS

            var libroCompras = handler.GenerateLibroCompras();
            path = libroCompras.Firmar(handler.nombreCertificado, "out\\temp\\", handler.serialKEY);

            MessageBox.Show("Libro compras guardado en " + path);
            #endregion
            //MessageBox.Show("Documento generado exitosamente en " + path);

            #region Factura de Compra Electrónica
            folios.Clear();
            pathFiles.Clear();

            folios.Add(18); //factura de compra
            folios.Add(32); //NC
            folios.Add(22); //ND    
            nAtencion = "1092647";

            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.FacturaCompraElectronica;
            handler.Folio = folios[0];
            handler.idDte = "A_" + folios[0];
            handler.casoPruebas = "CASO " + nAtencion + "-1";
            dte = handler.GenerateDTE();

            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 521,
                Nombre = "Producto 1",
                Precio = 4301,
                Afecto = true,
                TipoImpuesto = ChileSystems.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 27,
                Nombre = "Producto 2",
                Precio = 2279,
                Afecto = true,
                TipoImpuesto = ChileSystems.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            /********************************/
            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaCreditoElectronica;
            handler.Folio = folios[1];
            handler.idDte = "C_" + folios[1];
            handler.casoPruebas = "CASO " + nAtencion + "-2";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 174,
                Nombre = "Producto 1",
                Precio = 4301,
                Afecto = true,
                TipoImpuesto = ChileSystems.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal
            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 9,
                Nombre = "Producto 2",
                Precio = 2279,
                Afecto = true,
                TipoImpuesto = ChileSystems.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            dte.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.CorrigeMontos,
                FechaDocumentoReferencia = DateTime.Now,
                FolioReferencia = folios[0].ToString(),
                IndicadorGlobal = 0,
                Numero = 2,
                RazonReferencia = "DEVOLUCIÓN DE MERCADERÍA ITEMS 1 Y 2",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.FacturaCompraElectronica
            });
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            handler.tipoDTE = ChileSystems.DTE.Engine.Enum.TipoDTE.DTEType.NotaDebitoElectronica;
            handler.Folio = folios[2];
            handler.idDte = "D_" + folios[2];
            handler.casoPruebas = "CASO " + nAtencion + "-3";
            dte = handler.GenerateDTE();
            detalles = new List<ItemBoleta>();
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 174,
                Nombre = "Producto 1",
                Precio = 4301,
                Afecto = true,
                TipoImpuesto = ChileSystems.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal

            });
            detalles.Add(new ItemBoleta()
            {
                Cantidad = 9,
                Nombre = "Producto 2",
                Precio = 2279,
                Afecto = true,
                TipoImpuesto = ChileSystems.DTE.Engine.Enum.TipoImpuesto.TipoImpuestoEnum.IVARetenidoTotal
            });
            handler.GenerateDetails(dte, detalles);
            handler.Referencias(dte);
            dte.Documento.Referencias.Add(new ChileSystems.DTE.Engine.Documento.Referencia()
            {
                CodigoReferencia = ChileSystems.DTE.Engine.Enum.TipoReferencia.TipoReferenciaEnum.AnulaDocumentoReferencia,
                FechaDocumentoReferencia = DateTime.Now,
                FolioReferencia = folios[1].ToString(),
                IndicadorGlobal = 0,
                Numero = 2,
                RazonReferencia = "ANULA NOTA DE CREDITO",
                TipoDocumento = ChileSystems.DTE.Engine.Enum.TipoDTE.TipoReferencia.NotaCreditoElectronica
            });
            path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");
            pathFiles.Add(path);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.DTE, ChileSystems.DTE.Engine.XML.Schemas.DTE);
            /********************************/

            dtes = new List<ChileSystems.DTE.Engine.Documento.DTE>();
            xmlDtes = new List<string>();
            foreach (string pathFile in pathFiles)
            {
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                var dteAux = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);
                dtes.Add(dteAux);
                xmlDtes.Add(xml);
            }
            EnvioSII = handler.GenerarEnvioDTEToSII(dtes, xmlDtes);
            path = EnvioSII.Firmar(handler.nombreCertificado, handler.serialKEY, true);
            handler.Validate(path, SIMPLE_SDK.Security.Firma.Firma.TipoXML.Envio, ChileSystems.DTE.Engine.XML.Schemas.EnvioDTE);
            MessageBox.Show("Envío generado exitosamente en " + path);
            #endregion
        }

        private void botonIntercambio_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            string pathFile = openFileDialog1.FileName;
            string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
            var envio = ChileSystems.DTE.Engine.XML.XmlHandler.TryDeserializeFromString<ChileSystems.DTE.Engine.Envio.EnvioDTE>(xml);

            /*Respuesta de Intercambio*/
            var filePath = handler.GenerarRespuestaEnvio(envio.SetDTE.DTEs, "ACD");
            MessageBox.Show("Respuesta de Intercambio " + filePath);

            /*Recibo de mercaderías*/
            filePath = handler.AcuseReciboMercaderias(envio.SetDTE.DTEs[0]);
            MessageBox.Show("Acuse recibo " + filePath);

            /*Aprobación comercial de documento*/
            filePath = handler.ResponderDTE(0, envio.SetDTE.DTEs[0], "PRUEBA");
            MessageBox.Show("Aprobación comercial " + filePath);

           
        }

        private void botonCesion_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                /*La cesión se debe realizar a partir de un DTE existente
                 Para ello, se carga el correspondiente XML. Puede ser un <EnvioDTE> o <DTE>,
                 Sin embargo, al usar el primero, hay que hacer la respectiva modificación 
                 de Tipo en XmlHandler.DeserializeFromString<T>*/

                string pathFile = openFileDialog1.FileName;
                string xml = File.ReadAllText(pathFile, Encoding.GetEncoding("ISO-8859-1"));
                

                /*Creo el objeto AEC*/
                var AEC = new SIMPLE_SDK.Cesion.AEC();

                /*Creo el objeto DteCedido a partir del XML leído. En caso del XML haber sido del
                 tipo <EnvioDTE>, hay que rescatar sólo el XML desde el tag <DTE>, según el DTE que
                 se quiera ceder.

                 La variable xmlDteCedido me indica el Path donde está el DteCedido firmado.
                 */

                var dteCedido = new SIMPLE_SDK.Cesion.DTECedido(xml);
                var xmlDteCedido = dteCedido.Firmar(handler.nombreCertificado, out string message);


                /*Creo el objeto cesion a partir de DTE leído, se le indica el número de secuencia de
                  la cesión. Pueden existir varias cesiones.*/
                var dte = ChileSystems.DTE.Engine.XML.XmlHandler.DeserializeFromString<ChileSystems.DTE.Engine.Documento.DTE>(xml);
                var cesion = new SIMPLE_SDK.Cesion.Cesion(dte, 1);

                /*Datos del factoring*/
                var cesionario = new SIMPLE_SDK.Cesion.Cesionario()
                {
                    Direccion = "Dirección Cesionario",
                    eMail = "Email Cesionario",
                    RazonSocial = "Factoring LTDA",
                    RUT = "11111111-1"
                };

                var cedente = new SIMPLE_SDK.Cesion.Cedente()
                {
                    RUT = dte.Documento.Encabezado.Emisor.Rut,
                    RazonSocial = dte.Documento.Encabezado.Emisor.RazonSocial,
                    Direccion = dte.Documento.Encabezado.Emisor.DireccionOrigen +", " + dte.Documento.Encabezado.Emisor.ComunaOrigen,
                    eMail = dte.Documento.Encabezado.Emisor.CorreoElectronico,
                    RUTsAutorizados = new List<SIMPLE_SDK.Cesion.RUTAutorizado>()
                    {
                        new SIMPLE_SDK.Cesion.RUTAutorizado()
                        {
                            Nombre = "Nombre Autorizado",
                            RUT = "RUT Autorizado"
                        }
                    }
                };

                string declaracionJurada = string.Format(
                 @"Se declara bajo juramento que {0}, RUT {1} ha puesto a disposición del 
                    cesionario {2}, RUT {3}, el o los documentos donde constan los recibos 
                    de las mercaderías entregadas o servicios prestados, entregados por parte 
                    del deudor de la factura {4}, RUT {5}, de acuerdo a lo establecido en la 
                    Ley N° 19.983", 
                 cedente.RazonSocial, 
                 cedente.RUT, 
                 cesionario.RazonSocial,
                 cesionario.RUT,
                 dte.Documento.Encabezado.Receptor.RazonSocial,
                 dte.Documento.Encabezado.Receptor.Rut);

                cedente.DeclaracionJurada = declaracionJurada;

                cesion.DocumentoCesion.Cedente = cedente;
                cesion.DocumentoCesion.Cesionario = cesionario;

                /*la variable cesionXML contiene el path de la cesión firmada*/
                var cesionXML = cesion.Firmar(handler.nombreCertificado, out message);

                AEC.DocumentoAEC.Caratula = new SIMPLE_SDK.Cesion.Caratula()
                {
                    MailContacto = cedente.eMail,
                    NombreContacto = cedente.RUTsAutorizados[0].Nombre,
                    RutCedente = cedente.RUT,
                    RutCesionario = cesionario.RUT,
                    TmstFirmaEnvio = DateTime.Now
                };

                /*Las cesiones y el Dte cedido, deben agregarse al objeto AEC como strings*/
                AEC.signedXMLCedido = File.ReadAllText(xmlDteCedido, Encoding.GetEncoding("ISO-8859-1"));
                AEC.signedXMLCesion.Add(File.ReadAllText(cesionXML, Encoding.GetEncoding("ISO-8859-1")));
                AEC.DocumentoAEC.ID = "ID_TEST";
                var filePathAEC = AEC.Firmar(handler.nombreCertificado, out message);
                File.Delete(xmlDteCedido);
                File.Delete(cesionXML);

                MessageBox.Show("AEC generado exitosamente en " + filePathAEC);

                //var path = handler.TimbrarYFirmarXMLDTE(dte, "out\\temp\\", "out\\caf\\");

            }

        }
    }
}
