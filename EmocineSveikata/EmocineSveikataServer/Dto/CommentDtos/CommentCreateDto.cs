﻿namespace EmocineSveikataServer.Dto.CommentDto
{
	public class CommentCreateDto
	{
		public string Content { get; set; } = string.Empty;
		public int CreatorUserId { get; set; }
	}

}
