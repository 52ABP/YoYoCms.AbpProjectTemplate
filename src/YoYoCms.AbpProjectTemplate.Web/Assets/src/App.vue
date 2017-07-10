<style rel="styleesheet" lang="scss">
    @import "common/style";
    @import "./vendor/flags/famfamfam-flags.css";

    /* 进入和离开动画可以分别 */
    /* 设置不同的持续时间(duration)和动画函数(timing function) */
    .slide-fade-enter-active {
        transition: all .3s ease;
    }
    .slide-fade-leave-active {
        transition: all .8s cubic-bezier(1.0, 0.5, 0.8, 1.0);
    }
    .slide-fade-enter, .slide-fade-leave-to
        /* .slide-fade-leave-active 在 <2.1.8 中 */ {
        transform: translateX(10px);
        opacity: 0;
    }
</style>
<template>
    <div id="app">
        <router-view></router-view>
    </div>
</template>

<script>
    import store from './store'
    import './filters/timeFilter'
    export default {
        name: 'app',
        store,
    }
</script>
