module.exports = {
  'techno-grill-api': {
    input: 'http://localhost:7000/swagger/v1/swagger.json',
    output: {
      target: 'src/typings/api.gen.ts',
      client: 'react-query',

      override: {
        mutator: {
          path: './src/api/mutator/custom-instance.ts',
          name: 'customInstance',
        },
        useNativeEnums: true,
      },
    }
  }
}