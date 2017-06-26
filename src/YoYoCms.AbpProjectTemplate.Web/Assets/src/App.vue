<style rel="styleesheet" lang="scss">
    @import "common/style";
</style>
<template>
    <div id="app">
        <router-view></router-view>
    </div>
</template>

<script>
    import store from './store'
    export default {
        name: 'app',
        store
    }
</script>
