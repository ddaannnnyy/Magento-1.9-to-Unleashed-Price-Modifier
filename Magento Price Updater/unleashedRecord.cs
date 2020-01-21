using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dapper;

namespace Magento_Price_Updater
{
    public class unleashedRecord
    {
        /// <summary>
        /// loader for unleashed records that brings them in with a stream reader and then has them inserted into the database
        /// </summary>
        /// <param name="file">unleashed .csv file to load into the stream reader. currently this is pretty hardcoded.</param>
        /// <returns>string[] _values A string array of all records read from the file</returns>
        public string[] unleashedRecordLoader(string file)
        {
            StreamReader sr = new StreamReader(file);

            string strLine = string.Empty; //placeholder string for the line being currently read by the reader
            string query = string.Empty; 
            string[] _values = null; //string array for records to be returned in

            int currentRecord = 0; //used to not import the first line of data i.e. the headings, I don't want them in the database when appending data
            
            #region unleashed variable declarations
            string unleashedHeadings = "productCode,productDescription,notes,barcode,units,minStockAlertLevel,maxStockAlertLevel,binLocation,labelTemplate,supplierCode,supplierName,supplierProductCode,defaultPurchasePrice,minimumOrderQuantity,minimumSaleQuantity,defaultSellPrice,minimumSellPrice,sellPrice1,sellPrice2,sellPrice3,sellPrice4,sellPrice5,sellPrice6,sellPrice7,sellPrice8,sellPrice9,sellPrice10,packSize,weight,width,height,depth,lastCost,neverDiminishing,productGroup,salesAccount,COGSAccount,purchaseAccount,purchaseTaxType,purchaseTaxRate,salesTaxType,saleTaxRate,isAssembledProduct,isComponent,isObsoleted,isSellable,isApiProduct,apiProductType";
            string datafield0; //product code
            string datafield1; //product description
            string datafield2; //notes
            string datafield3; //barcode
            string datafield4; //units
            string datafield5; //min stock alert level
            string datafield6; //max stock alert level
            string datafield7; //bin location
            string datafield8; //label template
            string datafield9; //supplier code
            string datafield10;//supplier name
            string datafield11;//supplier product code
            double datafield12;//default purchase price
            string datafield13;//minimum order quantity
            string datafield14;//minimum sale quantity
            double datafield15;//default sell price
            double datafield16;//minimum sell price 
            double datafield17;//sell price 1
            double datafield18;//sell price 2
            double datafield19;//sell price 3
            double datafield20;//sell price 4
            double datafield21;//sell price 5
            double datafield22;//sell price 6
            double datafield23;//sell price 7
            double datafield24;//sell price 8
            double datafield25;//sell price 9
            double datafield26;//sell price 10
            string datafield27;//pack size
            string datafield28;//weight
            string datafield29;//width
            string datafield30;//height
            string datafield31;//depth
            double datafield32;//last cost
            string datafield33;//never diminishing
            string datafield34;//product group
            string datafield35;//sales account
            string datafield36;//COGS account
            string datafield37;//purchase account
            string datafield38;//purchase tax type
            string datafield39;//purchase tax rate
            string datafield40;//sale tax type
            string datafield41;//sale tax rate
            string datafield42;//is assembled product
            string datafield43;//is component
            string datafield44;//is obsoleted
            string datafield45;//is sellable
            string datafield46;//is api product
            string datafield47;//api product type
            #endregion


            try
            {
                while (!sr.EndOfStream) //while there is still data to read
                {
                    strLine = sr.ReadLine();
                    _values = strLine.Split(','); //currently hardcoded for csvs because it's only made for one specific file

                    if (currentRecord != 0)//do not read and import headings
                    {                                                
                        #region unleashed binding datafields
                        datafield0 = _values[0];
                        datafield1 = _values[1];
                        datafield2 = _values[2];
                        datafield3 = _values[3];
                        datafield4 = _values[4];
                        datafield5 = _values[5];
                        datafield6 = _values[6];
                        datafield7 = _values[7];
                        datafield8 = _values[8];
                        datafield9 = _values[9];
                        datafield10 = _values[10];
                        datafield11 = _values[11];
                        try { datafield12 = double.Parse(_values[12]); } catch { datafield12 = 0; }
                        datafield13 = _values[13];
                        datafield14 = _values[14];
                        try { datafield15 = double.Parse(_values[15]); } catch { datafield15 = 0; }
                        try { datafield16 = double.Parse(_values[16]); } catch { datafield16 = 0; }
                        try { datafield17 = double.Parse(_values[17]); } catch { datafield17 = 0; }
                        try { datafield18 = double.Parse(_values[18]); } catch { datafield18 = 0; }
                        try { datafield19 = double.Parse(_values[19]); } catch { datafield19 = 0; }
                        try { datafield20 = double.Parse(_values[20]); } catch { datafield20 = 0; }
                        try { datafield21 = double.Parse(_values[21]); } catch { datafield21 = 0; }
                        try { datafield22 = double.Parse(_values[22]); } catch { datafield22 = 0; }
                        try { datafield23 = double.Parse(_values[23]); } catch { datafield23 = 0; }
                        try { datafield24 = double.Parse(_values[24]); } catch { datafield24 = 0; }
                        try { datafield25 = double.Parse(_values[25]); } catch { datafield25 = 0; }
                        try { datafield26 = double.Parse(_values[26]); } catch { datafield26 = 0; }
                        datafield27 = _values[27];
                        datafield28 = _values[28];
                        datafield29 = _values[29];
                        datafield30 = _values[30];
                        datafield31 = _values[31];
                        try { datafield32 = double.Parse(_values[32]); } catch { datafield32 = 0; }
                        datafield33 = _values[33];
                        datafield34 = _values[34];
                        datafield35 = _values[35];
                        datafield36 = _values[36];
                        datafield37 = _values[37];
                        datafield38 = _values[38];
                        datafield39 = _values[39];
                        datafield40 = _values[40];
                        datafield41 = _values[41];
                        datafield42 = _values[42];
                        datafield43 = _values[43];
                        datafield44 = _values[44];
                        datafield45 = _values[45];
                        datafield46 = _values[46];
                        datafield47 = _values[47];

                        #endregion
                        #region building query string
                        //strings needs to be surrounded like 'this' and doubles do not
                        query =
                            "'" +   datafield0 + "'" + ","
                           + "'" +  datafield1 + "'" + "," //string are formatted like this 'STRING' for sqlite
                           + "'" +  datafield2 + "'" + ","
                           + "'" +  datafield3 + "'" + ","
                           + "'" +  datafield4 + "'" + ","
                           + "'" +  datafield5 + "'" + ","
                           + "'" +  datafield6 + "'" + ","
                           + "'" +  datafield7 + "'" + ","
                           + "'" +  datafield8 + "'" + ","
                           + "'" +  datafield9 + "'" + ","
                           + "'" +  datafield10 + "'" + ","
                           + "'" +  datafield11 + "'" + ","
                           +        datafield12 + ","        //doubles are formatted like this 10.06 for sqlite (note no ' surrounding)
                           + "'" +  datafield13 + "'" + ","
                           + "'" +  datafield14 + "'" + ","
                           +        datafield15 + ","
                           +        datafield16 + ","
                           +        datafield17 + ","
                           +        datafield18 + ","
                           +        datafield19 + ","
                           +        datafield20 + ","
                           +        datafield21 + ","
                           +        datafield22 + ","
                           +        datafield23 + ","
                           +        datafield24 + ","
                           +        datafield25 + ","
                           +        datafield26 + ","
                           + "'" +  datafield27 + "'" + ","
                           + "'" +  datafield28 + "'" + ","
                           + "'" +  datafield29 + "'" + ","
                           + "'" +  datafield30 + "'" + ","
                           + "'" +  datafield31 + "'" + ","
                           +        datafield32 + ","
                           + "'" +  datafield33 + "'" + ","
                           + "'" +  datafield34 + "'" + ","
                           + "'" +  datafield35 + "'" + ","
                           + "'" +  datafield36 + "'" + ","
                           + "'" +  datafield37 + "'" + ","
                           + "'" +  datafield38 + "'" + ","
                           + "'" +  datafield39 + "'" + ","
                           + "'" +  datafield40 + "'" + ","
                           + "'" +  datafield41 + "'" + ","
                           + "'" +  datafield42 + "'" + ","
                           + "'" +  datafield43 + "'" + ","
                           + "'" +  datafield44 + "'" + ","
                           + "'" +  datafield45 + "'" + ","
                           + "'" +  datafield46 + "'" + ","
                           + "'" +  datafield47 + "'";
                        #endregion

                        DatabaseUtil.insertData("unleashedImport",unleashedHeadings,query); //insert data to database
                    }

                    currentRecord++; //increment the current record
                }
                sr.Close(); //remember to close the reader when you're finished. I always forget this, there's probably a way to just use a using but this is how I was taught.
                return _values;
            }
            catch (Exception ex)
            {

                FileUtil.writeExeptionToFile(ex.Message);
                currentRecord++; //even on a fail I want to increase the record count
                return _values;
            }
        }
    }
}
