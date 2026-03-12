1. ```
   // 命令提示符
   npm create vue@latest
   ```

2. ```
   // 命令提示符
   npm install axios
   ```

3. ```
   // 在vite.config.js文件中解决跨域问题
   server: {
       proxy: {
         '/api': {
           target: 'https://0.0.0.0:7133',
           changeOrigin: true,
           secure: false,
         }
       }
     }
   ```

4. 在  ../src/views  中开发View，注意文件以View结尾

5. 在  ../src/router/index.js  中配置路由，注意要import文件

6. 修改  App.vue