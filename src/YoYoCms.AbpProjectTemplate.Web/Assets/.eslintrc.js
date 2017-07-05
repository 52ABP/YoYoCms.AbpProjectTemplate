module.exports = {
    root: true,
    parser: 'babel-eslint',
    parserOptions: {
        "ecmaVersion": 6,
        sourceType: 'module'
    },
    // https://github.com/feross/standard/blob/master/RULES.md#javascript-standard-style
    extends: 'standard',
    // required to lint *.vue files
    plugins: [
        'html'
    ],
    // add your custom rules here
    rules: {
        // allow paren-less arrow functions
        'arrow-parens': 0,
        // allow async-await
        'generator-star-spacing': 0,
        // change indent to 4 spaces
        'indent': ['error', 4],
        // allow debugger during development
        'no-debugger': process.env.NODE_ENV === 'production' ? 2 : 0,
        "no-unexpected-multiline": 0,
        "no-sequences": 0,
        "comma-dangle": 0,
        "eqeqeq": 0,
        "curly": [0, 'multi'],
        "space-before-function-paren": 0,
        'prefer-promise-reject-errors': 0,
        'camelcase': 0,
        'eol-last': 0
    },
    globals: {
        'abp': true,
        'alert': true,
        'fetch': true,
        'localStorage': true,
        'jquery': true,
        '$': true,
        'Exception': true,
        'lang': true,
        'Waves': true
    }
}
