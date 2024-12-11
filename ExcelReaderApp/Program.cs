using System;
using System.Collections.Generic;
using ExcelReaderApp;
using Newtonsoft.Json;
using OfficeOpenXml; // For reading Excel files

class Program
{
    static void Main(string[] args)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //EPPlus (for free use cases)
        try
        {
            string filePath = "C:/Users/PB123XY/Downloads/CopyFieldsPOCTemplate.xlsx"; //Excel file to be read

            ExcelReader excelReader = new ExcelReader(filePath);
            var excelData = excelReader.ReadExcelData();
            var requestBody = GetGroupDataImportRequestBodyDetail(excelData);
            string jsonString = JsonConvert.SerializeObject(requestBody, Formatting.Indented);
            Console.WriteLine("Generated JSON:");
            Console.WriteLine(jsonString);
            Console.WriteLine("\nJSON Complete. Press any key to exit.");
            Console.ReadKey();  
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    public static RequestBody.Rootobject GetGroupDataImportRequestBodyDetail(object[,] data)
    {
        var retSRMReqbody = new RequestBody.Rootobject();
        retSRMReqbody.fieldsRequest = new List<RequestBody.Fieldsrequest>();

        int countRows = data.GetLength(0); 
        RequestBody.Field groupField = null;
        List<RequestBody.Field> childFields = new List<RequestBody.Field>();
        RequestBody.Fieldsrequest fieldsRequestSRM = null;

        for (int i = 0; i < countRows; i++)
        {
            bool newGroup = Convert.ToString(data[i, 1]) == "Y"; // newGroup is true if Parent? = 'Y'
            if (newGroup)
            {
                if (groupField != null)
                {
                    if (childFields.Count > 0)
                    {
                        groupField.fields = childFields;
                    }
                    else
                    {
                        groupField.fields = null;
                    }
                    fieldsRequestSRM.groupField = groupField;
                    retSRMReqbody.fieldsRequest.Add(fieldsRequestSRM);
                }
                groupField = new RequestBody.Field
                {
                    fieldId = Convert.ToString(data[i, 0]),
                    value = Convert.ToString(data[i, 8]), // TBD - since Nested Sub Group? column deleted
                    isGroup = true 
                };
                childFields = new List<RequestBody.Field>();
                fieldsRequestSRM = new RequestBody.Fieldsrequest();
            }
            else
            {
                var field = new RequestBody.Field
                {
                    fieldId = Convert.ToString(data[i, 0]),
                    value = Convert.ToString(data[i, 8]), // TBD - since Nested Sub Group? column deleted
                    isGroup = false 
                };
                field.fields = null;  
                childFields.Add(field);
            }
        }

        if (groupField != null)
        {
            if (childFields.Count > 0)
            {
                groupField.fields = childFields;
            }
            else
            {
                groupField.fields = null;
            }
            fieldsRequestSRM.groupField = groupField;
            retSRMReqbody.fieldsRequest.Add(fieldsRequestSRM);
        }

        return retSRMReqbody;
    }
}

