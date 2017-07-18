<!--页面选项卡-->
<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-tabs v-model="currVal" type="card" @tab-click="tabClick" @tab-remove="tabRemove">
        <el-tab-pane :closable="index !== 0"
                     :key="item.name"
                     v-for="(item, index) in data"
                     :label="item.displayName"
                     :name="item.name">
        </el-tab-pane>
    </el-tabs>
</template>

<script>
    export default {
        data() {
            return {
                data: [],
                currVal: ''
            }
        },
        watch: {
            '$store.state.index.pageTab'(val) {
                this.setCurrVal(val)
            }
        },
        created() {
            this.setCurrVal(this.$store.state.index.pageTab)
        },
        activated() {
        },
        methods: {
            tabClick(vm) {
                let item = void 0
                this.data.forEach((itemData) => {
                    if (itemData.name === vm.name) item = itemData
                })
                this.$router.push({name: item.name, query: item.query, params: item.params})
            },
            setCurrVal(val) {
                this.data = val
                val.forEach((item) => {
                    if (item.isActive) this.currVal = item.name
                })
            },
            tabRemove(name) {
                let currItem
                let currItemIndex
                this.data.forEach((item, index) => {
                    if (item.name === name) {
                        currItem = item
                        currItemIndex = index
                    }
                })
                this.$store.dispatch('delPageTab', {name})
                // 如果当前是激活的tab 则跳转到工作台
                if (currItem.isActive) {
                    let preItem = this.data[currItemIndex - 1]
                    this.currVal = preItem.name
                    this.$router.push({name: preItem.name, query: preItem.query, params: preItem.params})
                }
            }
        },
        components: {}
    }
</script>
