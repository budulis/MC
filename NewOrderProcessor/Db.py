import pyodbc

class Db(object):
        
    def GetOrderIds(self):
        cnxn = pyodbc.connect(r'Driver={SQL Server};Server=.\SQLEXPRESS;Database=McStore;Trusted_Connection=yes;')
        cursor = cnxn.cursor()
        cursor.execute("SELECT OrderId,Products FROM Orders")

        orders = list()
        
        while 1:
            row = cursor.fetchone()
            if not row:
                break
            orders.append((row.OrderId,row.Products))

        cnxn.close()
        return orders

    def InsertProduct(self, id, name, price, description=None):
        cnxn = pyodbc.connect(r'Driver={SQL Server};Server=.\SQLEXPRESS;Database=McStore;Trusted_Connection=yes;')
        cnxn.execute("INSERT INTO [McStore].[dbo].[Products]VALUES (?, ?, ?, ?)",id,name,price,description)
        cnxn.commit()

    def insert_reciept_info(self, reciept):
        cnxn = pyodbc.connect(r'Driver={SQL Server};Server=.\SQLEXPRESS;Database=McStore;Trusted_Connection=yes;')
        cnxn.execute("INSERT INTO [McStore].[dbo].[Receipts]VALUES(?,?,?,?,?,?,DEFAULT,?)",reciept)
        cnxn.commit()

