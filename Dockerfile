#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# 添加一个基于sdk6.0镜像 
FROM mcr.microsoft.com/dotnet/sdk:6.0
#切换工作目录下
WORKDIR /app  
#对外端口5011
EXPOSE 5000
#当前目录下所有的文件复制到docker的app下
COPY .  /app 
#注意将location换成*号否则将拒绝访问
ENTRYPOINT ["dotnet", "Vitamin.Host.dll","--urls","http://*:5000"]

# docker build -t myblog:1.0 -f Dockerfile .
# docker run -d -it -v wwwroot/app/Files:/Files -p 5050:5000 --network bridge --name blog myblog:1.0 --restart always 
