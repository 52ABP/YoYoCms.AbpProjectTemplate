<!--语言和国旗-->
<style rel="styleesheet" lang="scss">
    .components-select__language-currFlag {
        vertical-align: middle;
        height: auto;
    }
</style>

<template>
    <article class="components-select__language-currFlag">
        <el-select :clearable="clearable" v-model="currVal" placeholder="请选择" @change="selOnChange">
            <el-option
                    v-for="item in data"
                    :key="item.id"
                    :label="item.displayName"
                    :value="item.name">
                <i :class="item.icon"></i>
                {{item.displayName}}
            </el-option>

        </el-select>
        <i :class="currFlag" style="vertical-align: middle"></i>
    </article>
</template>

<script>
    import languageService from '../../services/common/languageService'
    export default {
        props: {
            value: String,
            onChange: Function,
            clearable: {
                type: Boolean,
                default: false
            },
            flag: String,
            displayName: String,
        },
        data() {
            return {
                currVal: this.value,
                data: [],
                currFlag: void 0,
            }
        },
        watch: {
            'value'(val) {
                if (val != this.currVal) this.currVal = val
            },
            'currVal'(val) {
                this.$emit('input', val)

                // 查询currFlag
                this.data.forEach((item) => {
                    if (item.name === val) {
                        this.currFlag = item.icon
                        this.$emit('update:displayName', item.displayName)
                    }
                })
            },
            'currFlag'(val) {
                this.$emit('update:flag', val)
            }
        },
        async created() {
            this.data = (await languageService.getLanguages()).items
            if (!this.currFlag) {
                // 查询currFlag
                this.data.forEach((item) => {
                    if (item.name === this.currVal) {
                        this.currFlag = item.icon
                        this.$emit('update:displayName', item.displayName)
                    }
                })
            }
        },
        methods: {
            selOnChange() {
                this.onChange && this.onChange()
            }
        },
        components: {}
    }
</script>
