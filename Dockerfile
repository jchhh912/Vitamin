#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# ���һ������sdk6.0���� 
FROM mcr.microsoft.com/dotnet/sdk:6.0
#�л�����Ŀ¼��
WORKDIR /app  
#����˿�5011
EXPOSE 5000
#��ǰĿ¼�����е��ļ����Ƶ�docker��app��
COPY .  /app 
#ע�⽫location����*�ŷ��򽫾ܾ�����
ENTRYPOINT ["dotnet", "Vitamin.Host.dll","--urls","http://*:5000"]

# docker build -t myblog:1.0 -f Dockerfile .
# docker run -d -it -v wwwroot/app/Files:/Files -p 5050:5000 --network bridge --name blog myblog:1.0 --restart always 
