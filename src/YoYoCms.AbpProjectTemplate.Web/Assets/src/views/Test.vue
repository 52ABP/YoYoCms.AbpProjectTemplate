<style rel="styleesheet" lang="scss">
</style>

<template>
    <article>
        <Select2 :data="data" v-model="val"></Select2>
        <el-button @click="changeData">改变值</el-button>

        <el-button @click="changeSelected">改变选中</el-button>
    </article>
</template>

<script>
    import Select2 from '../components/select/common/Select2.vue'
    export default {
        data() {
            return {
                data: [{key: '1', value: 1}, {key: '2', value: 2}, {key: '222', value: 222}],
                val: 2
            }
        },
        created() {
        },
        activated() {
        },
        methods: {
            changeData() {
                this.data.push({key: '3', value: 3})
            },
            changeSelected () {
                this.val = 1
            }
        },
        components: {
            Select2
        }
    }
</script>
