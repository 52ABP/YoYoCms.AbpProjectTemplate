<!--时区-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-select v-model="value" placeholder="请选择" v-loading="loading" style="width: 500px">
        <el-option size="large"
                   v-for="item in data"
                   :key="item.value"
                   :label="item.name"
                   :value="item.value">
        </el-option>
    </el-select>
</template>

<script>
    import timingService from '../../services/common/timingService'
    export default {
        props: {
            value: String || Number
        },
        data() {
            return {
                loading: false,
                data: [],
                currVal: this.value
            }
        },
        watch: {
            'value'(val) {
                if (val != this.currVal) this.currVal = val
            },
            'currVal'(val) {
                this.$emit('input', val)
            }
        },
        created() {
            this.fetchData()
        },
        activated() {
        },
        methods: {
            async fetchData () {
                this.loading = true
                this.data = (await timingService.getTimezones({defaultTimezoneScope: 1})).items

                this.loading = false
            }
        },
        components: {}
    }
</script>
