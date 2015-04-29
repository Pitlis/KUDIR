using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KUDIR.Code
{
    public class ReportResources: Reports.IReportTemplates
    {
        public byte[] GetTemplate(string name)
        {
            switch (name)
            {
                case "Выручка.xlsx":
                    return Properties.Resources.Выручка;
                case "Отгрузка.xlsx":
                    return Properties.Resources.Отгрузка;
                case "Предоплата.xlsx":
                    return Properties.Resources.Предоплата;
                case "Кредитор.xlsx":
                    return Properties.Resources.Кредитор;
                case "ПодоходныйНалог.xlsx":
                    return Properties.Resources.ПодоходныйНалог;
                case "ПодоходныйНалогПеречислено.xlsx":
                    return Properties.Resources.ПодоходныйНалогПеречислено;
                case "Дивиденты.xlsx":
                    return Properties.Resources.Дивиденты;
                case "НалоговыйАгент.xlsx":
                    return Properties.Resources.НалоговыйАгент;
                case "Кооператив.xlsx":
                    return Properties.Resources.Кооператив;
                case "СтраховойВзнос.xlsx":
                    return Properties.Resources.СтраховойВзнос;
                case "ПеречисленныйСтраховойВзнос.xlsx":
                    return Properties.Resources.ПеречисленныйСтраховойВзнос;
                case "ПенсионныйВзнос.xlsx":
                    return Properties.Resources.ПенсионныйВзнос;
                case "ПенсионныйВзносПеречислено.xlsx":
                    return Properties.Resources.ПенсионныйВзносПеречислено;
                case "УчетРасходовФонда.xlsx":
                    return Properties.Resources.УчетРасходовФонда;
                case "УчетСтроений.xlsx":
                    return Properties.Resources.УчетСтроений;
                case "НезавершенныеСтроения.xlsx":
                    return Properties.Resources.НезавершенныеСтроения;
                case "ТоварыТС.xlsx":
                    return Properties.Resources.ТоварыТС;
                case "НДСприобретение.xlsx":
                    return Properties.Resources.НДСприобретение;
                case "НДСреализация.xlsx":
                    return Properties.Resources.НДСреализация;
                default:
                    break;
            }
            throw new NotImplementedException();
        }


    }
}
