using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magento_Price_Updater
{
    public class magentoRecord
    {
        /// <summary>
        /// loader for magento records that brings them in with a stream reader and then has them inserted into the database
        /// </summary>
        /// <param name="file">magento .csv file to load into the stream reader. currently this is pretty hardcoded.</param>
        /// <returns>string[] _values A string array of all records read from the file</returns>
        public string[] unleashedRecordLoader(string file)
        {
            StreamReader sr = new StreamReader(file);

            string strLine = string.Empty;
            string query = string.Empty;
            string[] _values = null;

            int currentRecord = 0;

            #region magento variable declariations
            string magentoHeadings = "sku,_store,_attribute_set,_type,_root_category,_product_websites,name,price";

            string datafield0;
            string datafield1;
            string datafield2;
            string datafield3;
            string datafield4;
            string datafield5;
            string datafield6;
            double datafield7;

            string mostRecentSKU = string.Empty;
            double mostRecentPrice = 0;
            string mostRecentName = string.Empty;
            #endregion

            while (!sr.EndOfStream)
            {
                strLine = sr.ReadLine();
                _values = strLine.Split(',');

                if (currentRecord != 0)
                {
                    if (_values[0] != "") //this will only find AU records, as natively NZ records don't have skus on their rows (no products exist on NZ but NOT on AU)
                    {
                        try
                        {
                            #region magento binding datafields
                            datafield0 = _values[0];
                            datafield1 = "default"; //forces default store condition on first line of product stack, this is a hard code for my specific use of this
                            datafield2 = _values[2];
                            datafield3 = _values[3];
                            datafield4 = _values[4];
                            datafield5 = _values[5];
                            datafield6 = _values[6];
                            datafield7 = double.Parse(_values[7]);

                            mostRecentSKU = datafield0;
                            mostRecentPrice = datafield7;
                            mostRecentName = datafield6;
                            #endregion
                            #region building query string
                            query = "'" + datafield0 + "'" + ", "
                                  + "'" + datafield1 + "'" + ", "
                                  + "'" + datafield2 + "'" + ", "
                                  + "'" + datafield3 + "'" + ", "
                                  + "'" + datafield4 + "'" + ", "
                                  + "'" + datafield5 + "'" + ", "
                                  + "'" + datafield6 + "'" + ", "
                                  + "'" + datafield7 + "'";
                            #endregion

                            if (_values[0] != "" && _values[5] == "base") //a second check that should only match AU records before writing to the db
                            {
                                DatabaseUtil.insertData("magentoImport", magentoHeadings, query);
                            }
                        }
                        catch (Exception ex)
                        {
                            FileUtil.writeExeptionToFile(ex.Message);
                            currentRecord++;
                            return _values;
                        }
                    }
                    else //this will find any child record under a parent AU. that is it will find NZ orders
                    {
                        try
                        {
                            #region magento binding datafields
                            datafield0 = mostRecentSKU;
                            datafield1 = "default_nz"; //forces default store condition on first line of product stack, this is a hard code for my specific use of this
                            datafield2 = _values[2];
                            datafield3 = _values[3];
                            datafield4 = _values[4];
                            datafield5 = _values[5];
                            datafield6 = mostRecentName;
                            datafield7 = double.Parse(_values[7]);

                            mostRecentSKU = datafield0;
                            mostRecentPrice = datafield7;
                            mostRecentName = datafield6;
                            #endregion
                            #region building query string
                            query = "'" + datafield0 + "'" + ", "
                                  + "'" + datafield1 + "'" + ", "
                                  + "'" + datafield2 + "'" + ", "
                                  + "'" + datafield3 + "'" + ", "
                                  + "'" + datafield4 + "'" + ", "
                                  + "'" + datafield5 + "'" + ", "
                                  + "'" + datafield6 + "'" + ", "
                                  + "'" + datafield7 + "'";
                            #endregion

                            if (_values[5] == "newzealand") //a second check that should only match AU records before writing to the db
                            {
                                try
                                {
                                    DatabaseUtil.insertData("magentoImport", magentoHeadings, query);
                                    mostRecentSKU = datafield0; //records most recent sku written to the db
                                }
                                catch (Exception ex)
                                {
                                    FileUtil.writeExeptionToFile(ex.Message);
                                    mostRecentSKU = datafield0; //still record the most recent sku written to the db

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FileUtil.writeExeptionToFile(ex.Message);
                            currentRecord++;
                            return _values;

                        }
                    }

                } currentRecord++; //increments current record counter
            }

            sr.Close(); //remember to close the streamreader
            return _values;
        }

        public void updatePrice()
        {
            List<string> currentRecord = new List<string>();
            string getPriceQuery = string.Empty;
            string setPriceQuery = string.Empty;

            try
            {
                using (var db = DatabaseUtil.getConnection().OpenAndReturn())
                {
                    
                }
            }
            catch (Exception ex)
            {
                FileUtil.writeExeptionToFile(ex.Message);
            }
        }
    }
}
