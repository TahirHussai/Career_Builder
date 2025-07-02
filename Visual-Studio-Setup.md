# Visual Studio 2022 Multiple Startup Projects Setup

## 🎯 Step-by-Step Configuration

### **Step 1: Open Solution in Visual Studio 2022**
1. Open `CareerBuilder.sln` in Visual Studio 2022
2. Wait for the solution to fully load

### **Step 2: Configure Multiple Startup Projects**
1. **Right-click** on the solution name in Solution Explorer
2. Select **"Properties"** (or **"Set Startup Projects..."**)
3. In the Solution Property Pages dialog:
   - Select **"Multiple startup projects"**
   - Set both projects to **"Start"**:
     - ✅ **CareerBuilder.API** → **Start**
     - ✅ **CareerBuilder** → **Start**
4. Click **"OK"**

### **Step 3: Verify Launch Profiles**
1. **For CareerBuilder.API:**
   - Right-click project → Properties
   - Go to **Debug** tab
   - Ensure **"CareerBuilder.API"** profile is selected
   - Verify URL: `http://localhost:5047`

2. **For CareerBuilder:**
   - Right-click project → Properties  
   - Go to **Debug** tab
   - Ensure **"CareerBuilder"** profile is selected
   - Verify URL: `http://localhost:5254`

### **Step 4: Build and Run**
1. **Build Solution**: `Ctrl+Shift+B`
2. **Start Debugging**: `F5` or click **Start**
3. **Both projects should start simultaneously**

## 🔧 Troubleshooting

### **Issue: API Not Starting**
**Symptoms:** Only frontend opens, no API window
**Solutions:**
1. Check if API project is set to "Start" in startup projects
2. Verify API launch profile is correct
3. Check for port conflicts (5047)
4. Look at Output window for errors

### **Issue: Port Conflicts**
**Symptoms:** "Port already in use" errors
**Solutions:**
1. Change ports in `launchSettings.json`
2. Kill existing processes using the ports
3. Use different port numbers

### **Issue: Database Connection**
**Symptoms:** API starts but database errors
**Solutions:**
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Run database migrations: `Update-Database`

### **Issue: CORS Errors**
**Symptoms:** Frontend can't connect to API
**Solutions:**
1. Verify CORS configuration in API `Program.cs`
2. Check API URL in frontend configuration
3. Ensure both projects use correct ports

## 📊 Expected Behavior

### **When Working Correctly:**
1. **Press F5** → Two console windows open
2. **API Console**: Shows "Now listening on: http://localhost:5047"
3. **Frontend Console**: Shows "Now listening on: http://localhost:5254"
4. **Browser Opens**: Frontend at http://localhost:5254
5. **Swagger Available**: API docs at http://localhost:5047/swagger

### **Console Output Examples:**

**API Console (CareerBuilder.API):**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5047
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**Frontend Console (CareerBuilder):**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5254
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

## 🎯 Quick Verification

### **Test API is Running:**
1. Open browser to `http://localhost:5047/swagger`
2. Should see Swagger UI with API endpoints
3. Try the `/api/auth/login` endpoint

### **Test Frontend is Running:**
1. Open browser to `http://localhost:5254`
2. Should see CareerBuilder homepage
3. Try registering/logging in

### **Test Integration:**
1. Register a new user on frontend
2. Check API console for database activity
3. Login with the new user
4. Post a job and verify it appears

## 🚨 Common Visual Studio Issues

### **Issue: "Multiple startup projects" option grayed out**
**Solution:** Ensure solution is loaded completely, not individual projects

### **Issue: Projects start but immediately close**
**Solution:** Check for compilation errors in Error List

### **Issue: Only one project starts despite configuration**
**Solution:** 
1. Clean solution (`Build` → `Clean Solution`)
2. Rebuild solution (`Build` → `Rebuild Solution`)
3. Try again

### **Issue: Different ports than expected**
**Solution:** Check `launchSettings.json` files for correct port configuration

## 📝 Alternative Methods

### **Method 1: Command Line (Backup)**
If Visual Studio multiple startup doesn't work:
```cmd
# Terminal 1
cd CareerBuilder.API
dotnet run

# Terminal 2  
cd CareerBuilder
dotnet run
```

### **Method 2: Batch File**
```cmd
start-both.bat
```

### **Method 3: PowerShell Script**
```powershell
.\start-both.ps1
```

## ✅ Success Checklist

- [ ] Solution opens without errors
- [ ] Both projects set to "Start" in startup projects
- [ ] Launch profiles configured correctly
- [ ] Build succeeds without errors
- [ ] F5 starts both projects
- [ ] API accessible at http://localhost:5047/swagger
- [ ] Frontend accessible at http://localhost:5254
- [ ] Database connection working
- [ ] Authentication flow working
- [ ] Job posting/application flow working
