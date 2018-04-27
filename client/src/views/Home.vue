<template>
  <div class="home">
    <h1>Values Management System</h1>
    <form>
      <label for="newValue">
        <p>Add Value</p>
      </label>
      <input type="text" name="newValue" id="newValue" v-model="newValue">
      <input type="button" value="Add Value" @click="addValue">
    </form>
    <br>
    <p>Current Values</p>
    <ul v-for="v in values" :key="v">
      <li>
        <b>{{v}}</b> - <button @click="deleteValue(v)">X</button>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { HubConnection, TransportType } from "@aspnet/signalr";

export default Vue.extend({
  data: () => ({
    values: [],
    newValue: ""
  }),

  beforeMount: async function() {
    const apiBase = "https://localhost:5001";
    try {
      const response = await fetch(`${apiBase}/api/values`);
      const values: string[] = await response.json();
      this.values = values;
      const hubConnection = new HubConnection(`${apiBase}/hubs/Values`, {
        transport: TransportType.WebSockets
      });
      hubConnection.on("Add", (value: string) => {
        this.values.push(value);
      });
      hubConnection.on("Delete", (value: string) => {
        this.values = this.values.filter(v => v !== value);
      });
      hubConnection.start();
    } catch {
      alert("Unable to connect to API");
    }
  },

  methods: {
    addValue: async function() {
      const apiBase = "https://localhost:5001";
      try {
        const response = await fetch(`${apiBase}/api/values`, {
          body: JSON.stringify(this.newValue),
          headers: new Headers({
            "content-type": "application/json"
          }),
          method: "POST"
        });
        this.newValue = "";
      } catch {
        alert("Unable to add value to API");
      }
    },
    deleteValue: async function(value) {
      const apiBase = "https://localhost:5001";
      try {
        const response = await fetch(`${apiBase}/api/values/${this.values.indexOf(value)}`, {
          method: "DELETE"
        });
      } catch {
        alert("Unable to delete value from API");
      }
    }
  }
});
</script>

<style scoped>
  li {
    list-style: none;
  }
</style>
