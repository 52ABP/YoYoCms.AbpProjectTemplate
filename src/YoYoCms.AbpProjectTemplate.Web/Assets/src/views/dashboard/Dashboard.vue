<!--工作台-->
<style rel="styleesheet" lang="scss">
    .dashboard-container {
        background-color: #dbe8f8;
        padding: 15px;
        border-color: #a0c2ec;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1), 0 1px 2px rgba(0, 0, 0, 0.18);
        border-radius: 5px;
        color: #333;

        h1 {
            font-size: 24px;
            margin-top: 0;
        }
        p {
            font-size: 13px;
        }

        h2 {
            font-size: 22px;
        }
    }
</style>

<template>
    <article class="dashboard-container">
        <h1>欢迎使用ASP.NET Zero启动项目!</h1>
        <p>
            在这个演示系统，您可以使用左边的菜单功能，测试和了解以下模块的全部功能：用户管理、角色管理、系统设置。 主面板(Dashboard)页面只是用于演示目的，
            文本和数据没有什么含义(所有数据都是硬编码在浏览器客户端程序； 其中的MEMBER ACTIVITY图表，向服务器端请求简单随机数据用于展示，当您点击刷新按钮时，可以看到变化。)
        </p>

        <h2>项目介绍</h2>

        <div>本项目前端使用 vue+vuex+vue-router</div>
    </article>
</template>

<script>
    export default {
        name: 'dashboard-Dashboard',
        data() {
            return {}
        },
        mounted() {
            setTimeout(() => {
                abp.view.setContentLoading(false)
            }, 5e2)
        },
        components: {}
    }
</script>
