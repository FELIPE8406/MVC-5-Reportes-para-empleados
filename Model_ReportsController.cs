using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidades;
using System.ComponentModel;

namespace PortalEmpleados.Models
{

    public class ReportsViewModels
    {
        public List<Ent_Reports> reports = new List<Ent_Reports>();
    }
    public class ReportParamsViewModels
    {
        public List<Ent_ReportParam> parameters = new List<Ent_ReportParam>();
    }

    public class comprobantesPagoViewModels
    {
        [Display(Name = "Fecha de nómina")]
        [Required]
        public string Fecha { get; set; }

        public string Ind_Fec { get; set; }

        public string CodConv { get; set; }

        public string cod_cia { get; set; }

        public string CodSuc { get; set; }

        public string cod_cco_conv { get; set; }

        public string CodCco { get; set; }

        public string cod_cla1 { get; set; }

        public string cod_cla2 { get; set; }

        public string cod_cla3 { get; set; }

        public string CodEmp { get; set; }

        public string TipLiq { get; set; }

        public string Origen { get; set; }

        public comprobantesPagoViewModels()
        {
            Ind_Fec = "1";
            CodConv = "%";
            cod_cia = "%";
            CodSuc = "%";
            cod_cco_conv = "%";
            CodCco = "%";
            cod_cla1 = "%";
            cod_cla2 = "%";
            cod_cla3 = "%";
            TipLiq = "01";
            CodSuc = "%";
            Origen = "H";
        }

    }

    public class certificadosLaboralesViewModels
    {
        public string Cedula { get; set; }
        public string Sucursal { get; set; }
    }


    public class certificadoArusViewModels
    {
        public string Cedula { get; set; }
        public string Sucursal { get; set; }
        public string tip_ide { get; set; }        

    }

}
