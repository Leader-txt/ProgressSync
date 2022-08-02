# ProgressSync
TShock Plugin ——ProgressSync TShock插件——进度同步
## 命令
ps 权限 ProgressSync.admin 同步进度
ps reset 重置进度
## 配置文件栗子
``` json
{
  "Mysql IP地址": "localhost",
  "Mysql 端口": "3305",
  "库名": "data",
  "Mysql用户名": "root",
  "Mysql密码": "123456",
  "需要通知的服务器": [
    {
      "服务器IP": "127.0.0.1",
      "Rest端口": "7878",
      "Rest令牌": "a"
    }
  ]
}
```
