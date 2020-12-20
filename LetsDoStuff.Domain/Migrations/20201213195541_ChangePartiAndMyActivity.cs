﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace LetsDoStuff.Domain.Migrations
{
    public partial class ChangePartiAndMyActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityUser_Activities_PartisId",
                table: "ActivityUser");

            migrationBuilder.RenameColumn(
                name: "PartisId",
                table: "ActivityUser",
                newName: "ParticipationActivitiesId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityUser_PartisId",
                table: "ActivityUser",
                newName: "IX_ActivityUser_ParticipationActivitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityUser_Activities_ParticipationActivitiesId",
                table: "ActivityUser",
                column: "ParticipationActivitiesId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityUser_Activities_ParticipationActivitiesId",
                table: "ActivityUser");

            migrationBuilder.RenameColumn(
                name: "ParticipationActivitiesId",
                table: "ActivityUser",
                newName: "PartisId");

            migrationBuilder.RenameIndex(
                name: "IX_ActivityUser_ParticipationActivitiesId",
                table: "ActivityUser",
                newName: "IX_ActivityUser_PartisId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityUser_Activities_PartisId",
                table: "ActivityUser",
                column: "PartisId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}