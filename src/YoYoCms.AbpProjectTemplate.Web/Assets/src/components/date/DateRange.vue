<style rel="styleesheet" lang="scss">
</style>

<template>
    <el-date-picker :size="size" ref="container"
                    @change="onChange"
                    v-model="currVal"
                    type="daterange"
                    align="right"
                    :placeholder="L('DateRange')"
                    :picker-options="pickerOptions">
    </el-date-picker>
</template>

<script>
    // start和end 会返回形如 2017-6-29T00:00:00.000Z和2017-6-29T23:59:59.999Z 的格式
    export default {
        props: {
            start: Date, // 开始日期
            end: Date, // 结束日期
            onChange: {
                type: Function,
                default: () => {
                }
            },
            size: String
        },
        data() {
            return {
                pickerOptions: {
                    shortcuts: [{
                        text: '今天',
                        onClick(picker) {
                            const end = new Date()
                            const start = new Date()
                            picker.$emit('pick', [start, end])
                        }
                    }, {
                        text: '昨天',
                        onClick(picker) {
                            const end = new Date()
                            const start = new Date()
                            start.setDate(start.getDate() - 1)
                            picker.$emit('pick', [start, end])
                        }
                    }, {
                        text: '最近一周',
                        onClick(picker) {
                            const end = new Date()
                            const start = new Date()
                            start.setDate(start.getDate() - 6)
                            picker.$emit('pick', [start, end])
                        }
                    }, {
                        text: '最近一个月',
                        onClick(picker) {
                            const end = new Date()
                            const start = new Date()
                            start.setMonth(start.getMonth() - 1)
                            picker.$emit('pick', [start, end])
                        }
                    }, {
                        text: '最近三个月',
                        onClick(picker) {
                            const end = new Date()
                            const start = new Date()
                            start.setMonth(start.getMonth() - 3)
                            picker.$emit('pick', [start, end])
                        }
                    }]
                },
                currVal: [this.start, this.end]
            }
        },
        watch: {
            'currVal'(val) {
                val = val || new Array(2)
                if (val[0] == null) {
                    val[0] = new Date()
                    val[1] = new Date()
                }

                val[0].setHours(0, 0, 0, 0)
                val[1].setHours(23, 59, 59, 999)
                this.$emit('update:start', val[0])
                this.$emit('update:end', val[1])
            },
            'start' (val) {
                this.currVal = this.currVal || new Array(2)
                if (val !== this.currVal[0]) this.currVal[0] = val
            },
            'end' (val) {
                this.currVal = this.currVal || new Array(2)
                if (val !== this.currVal[1]) this.currVal[1] = val
            }
        },
    }
</script>
