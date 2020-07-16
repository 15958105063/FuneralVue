FROM microsoft/dotnet:3.1-aspnetcore-runtime    
COPY  . /publish    
WORKDIR /publish
EXPOSE 8081 
ENTRYPOINT ["dotnet", "Funeral.Core.dll"]    