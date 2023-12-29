from breeze_connect import BreezeConnect
import pyodbc 
from typing import Any
import json
import ast
from typing import Any
from dataclasses import dataclass
import json
import pandas as pd
import time 
import urllib
import sqlite3
from sqlalchemy import create_engine
from breeze_connect import BreezeConnect

text_file = open("C:\Hosts\ICICI_Key\key.txt", "r")
 
#read whole file to a string
data = text_file.read()
 
#close file
text_file.close()
 
print(data)
connection = pyodbc.connect('Driver={SQL Server};'
                      'Server=HAADVISRI\AGS;'
                      'Database=STOCK;'
                      'Trusted_Connection=yes;MARS_Connection=Yes')
engine= create_engine('mssql+pyodbc://sa:240149@HAADVISRI\AGS/STOCK?driver=SQL+Server+Native+Client+11.0')
#engine = create_engine('mssql://HAADVISRI\AGS/STOCK?trusted_connection=yes')
# Initialize SDK
breeze = BreezeConnect(api_key="6N4Hj74vE@668970816zP9K307YZ58Ff")

# Obtain your session key from https://api.icicidirect.com/apiuser/login?api_key=YOUR_API_KEY
# Incase your api-key has special characters(like +,=,!) then encode the api key before using in the url as shown below.
import urllib
print("https://api.icicidirect.com/apiuser/login?api_key="+urllib.parse.quote_plus("6N4Hj74vE@668970816zP9K307YZ58Ff"))

# Generate Session
breeze.generate_session(api_secret="iz1F27815220!290Ie8Ha459997J8376",
                        session_token=data)

# Generate ISO8601 Date/DateTime String
import datetime
iso_date_string = datetime.datetime.strptime("28/02/2021","%d/%m/%Y").isoformat()[:10] + 'T05:30:00.000Z'
iso_date_time_string = datetime.datetime.strptime("28/02/2021 23:59:59","%d/%m/%Y %H:%M:%S").isoformat()[:19] + '.000Z'



df = pd.read_sql(
      "SELECT * FROM [STOCK].[dbo].[AUTO_BUY_EQUTIES2] Where   RID is null and cast(DATE as Date)=cast(getdate() as Date)",
  engine,
  index_col='RID')
#print(df.head())
       
for index, row in df.iterrows():
        #print('"' + str(row['stock_code']) + '"')
        #print('"' + str(row['EX']) + '"')
        #print('"' + str(row['ORDERType']) + '"')
        #print('"' + str(row['STOPLoss']) + '"')
        #print('"' + str(row['Quanity']) + '"')
        #print('"' + str(row['BUY_PRICE']) + '"')
        print(str(row))
        results=breeze.place_order(stock_code='' + str(row['stock_code'])+ '',
                            exchange_code='' + str(row['EX']).lower()+ '',
                            product="cash",
                            action='' + str(row['Action']).lower()+ '',
                            order_type='' + str(row['ORDERType']).lower()+ '',
                            stoploss='' + str(row['STOPLoss']).lower()+ '',
                            quantity='' + str(row['Quanity']).lower()+ '',
                            price='' + str(row['BUY_PRICE']).lower()+ '',
                            validity="day"
                        )
       
        ticktsafter = json.dumps(results)
        studObj = (json.loads(ticktsafter))
        print(studObj)
        if studObj['Status']==200:
            print(studObj)
            sticktsafter = json.dumps(studObj['Success'])
            tudObj = (json.loads(sticktsafter))
            print(tudObj['order_id'])
            cursor = connection.cursor()         
            sql= "UPDATE [dbo].[AUTO_BUY_EQUTIES] set RID='"+tudObj['order_id']+"', Resposne='"+json.dumps(results)+"' where ID="+ str(row['ID'])+"";
            print(sql)
            cursor.execute(sql)
            sqlnew = """\
                EXEC InsertBuyStocks @STOCKCode=?, @Change=?,@ltp=?
                    """
            params = (str(str(row['stock_code'])),-1,-1)
            print(cursor.execute(sqlnew, params))
            connection.commit() 
            cursor.close()
        else:
            cursor = connection.cursor()   
            sql= "UPDATE [dbo].[AUTO_BUY_EQUTIES2] set Resposne='"+json.dumps(results)+"', RID=-1 where ID="+ str(row['ID'])+"";
            print(sql)
            cursor.execute(sql)
            connection.commit() 
            cursor.close()
        