### MySQL日志批量写入失败

**提示**

`The used command is not allowed with this MySQL version`

 **解决办法**

在数据库中执行:

```sql
SET GLOBAL local_infile = 'ON';
```



### 日志批量写入数据时出错

**提示**

`To use MySq1Bu1kLoader. Loca1=true, set AllowLoadLocalInfile =true in the connection string.`

**解决办法**

在后台项目 appsettings.json 中找到 MySQL 数据库链接配置中添加：`AllowLoadLocalInfile =true`

